using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Commands;

public record RemoverProdutoCommand(Guid Id) : IRequest<bool>;

public class RemoverProdutoCommandHandler : IRequestHandler<RemoverProdutoCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoverProdutoCommandHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoverProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto is null) return false;

        await _produtoRepository.RemoverAsync(produto);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
