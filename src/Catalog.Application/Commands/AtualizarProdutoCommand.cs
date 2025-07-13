using Catalog.Application.DTOs;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Commands;

public record AtualizarProdutoCommand(Guid Id, ProdutoInputDTO Produto) : IRequest<bool>;
public class AtualizarProdutoCommandHandler : IRequestHandler<AtualizarProdutoCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarProdutoCommandHandler(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AtualizarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto is null) return false;

        var dto = request.Produto;

        var categoria = await _categoriaRepository
            .ObterTodosAsync()
            .ContinueWith(t => t.Result.FirstOrDefault(c => c.Nome == dto.Categoria), cancellationToken);

        if (categoria is null)
        {
            categoria = new Domain.Entities.Categoria(dto.Categoria);
            await _categoriaRepository.AdicionarAsync(categoria);
        }

        produto.Atualizar(dto.Nome, dto.Descricao, dto.Preco, categoria.Id);
        await _produtoRepository.AtualizarAsync(produto);
        await _unitOfWork.CommitAsync();

        return true;
    }
}