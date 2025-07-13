using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Queries;
public record BuscarProdutoPorTermoQuery(string Termo) : IRequest<IEnumerable<Produto>>;

public class BuscarProdutoPorTermoHandler : IRequestHandler<BuscarProdutoPorTermoQuery, IEnumerable<Produto>>
{
    private readonly IProdutoRepository _repository;

    public BuscarProdutoPorTermoHandler(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Produto>> Handle(BuscarProdutoPorTermoQuery request, CancellationToken cancellationToken)
    {
        return await _repository.BuscarAsync(request.Termo);
    }
}