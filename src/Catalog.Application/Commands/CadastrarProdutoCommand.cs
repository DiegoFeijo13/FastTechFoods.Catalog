using Catalog.Application.DTOs;
using Catalog.Application.Validators;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Commands;

public record CadastrarProdutoCommand(ProdutoInputDTO Produto) : IRequest<bool>;
public class CadastrarProdutoCommandHandler : IRequestHandler<CadastrarProdutoCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarProdutoCommandHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CadastrarProdutoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Produto;

        var categoria = await _categoriaRepository.ObterPorNomeAsync(dto.Categoria);

        Guid categoriaId;

        if (categoria is null)
        {
            categoria = new Categoria(dto.Categoria);
            await _categoriaRepository.AdicionarAsync(categoria);
            categoriaId = categoria.Id;
        }
        else
        {
            categoriaId = categoria.Id;
        }

        var produto = new Produto(dto.Nome, dto.Descricao, dto.Preco, categoriaId);
        await _produtoRepository.AdicionarAsync(produto);
        await _unitOfWork.CommitAsync();

        return true;
    }     
}

