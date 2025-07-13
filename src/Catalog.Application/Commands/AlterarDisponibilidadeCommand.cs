using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Commands;

public record AlterarDisponibilidadeCommand(long Id, bool Disponivel) : IRequest<bool>;

public class AlterarDisponibilidadeCommandHandler : IRequestHandler<AlterarDisponibilidadeCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AlterarDisponibilidadeCommandHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AlterarDisponibilidadeCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto is null) return false;

        if (request.Disponivel)
            produto.TornarDisponivel();
        else
            produto.TornarIndisponivel();

        await _produtoRepository.AtualizarAsync(produto);
        await _unitOfWork.CommitAsync();

        return true;
    }
}