using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Queries;
public record ObterProdutoPorIdQuery(Guid Id) : IRequest<ProdutoOutputDTO?>;

public class ObterProdutoPorIdHandler : IRequestHandler<ObterProdutoPorIdQuery, ProdutoOutputDTO?>
{
    private readonly IProdutoRepository _repository;

    public ObterProdutoPorIdHandler(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProdutoOutputDTO?> Handle(ObterProdutoPorIdQuery request, CancellationToken cancellationToken)
    {
        var produto =  await _repository.ObterPorIdAsync(request.Id);
        if (produto == null) 
            return null;
        return new ProdutoOutputDTO(produto.Nome, produto.Descricao, produto.Preco);
    }

}