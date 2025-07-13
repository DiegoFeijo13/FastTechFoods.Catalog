using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Queries;

public record ObterTodosProdutosQuery() : IRequest<IEnumerable<ProdutoOutputDTO>>;

public class ObterTodosProdutosHandler : IRequestHandler<ObterTodosProdutosQuery, IEnumerable<ProdutoOutputDTO>>
{
    private readonly IProdutoRepository _repository;

    public ObterTodosProdutosHandler(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProdutoOutputDTO>> Handle(ObterTodosProdutosQuery request, CancellationToken cancellationToken)
    {
        var produtos =  await _repository.ObterTodosAsync();
        return produtos.Select(p =>
        {
            return new ProdutoOutputDTO(p.Nome, p.Descricao, p.Preco);
        });
    }
}