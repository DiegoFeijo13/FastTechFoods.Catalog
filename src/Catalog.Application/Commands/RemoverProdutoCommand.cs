using Catalog.Application.Events;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;
using Catalog.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Catalog.Application.Commands;

public record RemoverProdutoCommand(Guid Id) : IRequest<bool>;

public class RemoverProdutoCommandHandler : IRequestHandler<RemoverProdutoCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publish;

    public RemoverProdutoCommandHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IPublishEndpoint publish)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
        _publish = publish;
    }

    public async Task<bool> Handle(RemoverProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto is null) return false;

        await _produtoRepository.RemoverAsync(produto);
        await _unitOfWork.CommitAsync();

        await _publish.Publish<IProdutoRemovidoEvent>(new ProdutoRemovidoEvent
        {
            Id = produto.Id
        });

        return true;
    }
}
