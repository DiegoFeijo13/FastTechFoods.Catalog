using Catalog.Application.DTOs;
using Catalog.Application.Events;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;
using Catalog.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Catalog.Application.Commands;

public record CadastrarProdutoCommand(ProdutoInputDTO Produto) : IRequest<bool>;
public class CadastrarProdutoCommandHandler : IRequestHandler<CadastrarProdutoCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publish;

    public CadastrarProdutoCommandHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork, IPublishEndpoint publish)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
        _publish = publish;
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

        await _publish.Publish<IProdutoCadastradoEvent>(new ProdutoCadastradoEvent
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Categoria = categoria.Nome,
            Preco = produto.Preco
        });

        return true;
    }     
}

