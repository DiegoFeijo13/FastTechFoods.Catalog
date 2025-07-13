using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Queries;

public record ObterProdutosPorCategoriaQuery(string NomeCategoria) : IRequest<IEnumerable<ProdutoOutputDTO>>;
public class ObterProdutosPorCategoriaHandler : IRequestHandler<ObterProdutosPorCategoriaQuery, IEnumerable<ProdutoOutputDTO>>
{
    private readonly IProdutoRepository _produtoRepo;
    private readonly ICategoriaRepository _categoriaRepo;

    public ObterProdutosPorCategoriaHandler(IProdutoRepository produtoRepo, ICategoriaRepository categoriaRepo)
    {
        _produtoRepo = produtoRepo;
        _categoriaRepo = categoriaRepo;
    }

    public async Task<IEnumerable<ProdutoOutputDTO>> Handle(ObterProdutosPorCategoriaQuery request, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepo.ObterPorNomeAsync(request.NomeCategoria);
        if(categoria is null)
            return Enumerable.Empty<ProdutoOutputDTO>();

        var produtos = await _produtoRepo.ObterPorCategoriaAsync(categoria.Id!);

        return produtos.Select(p =>
        {
            return new ProdutoOutputDTO(p.Nome, p.Descricao, p.Preco);
        });
    }
}
