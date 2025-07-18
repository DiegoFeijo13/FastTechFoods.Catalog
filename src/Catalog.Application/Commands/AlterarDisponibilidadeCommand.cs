using Catalog.Application.Events;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;
using Catalog.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Catalog.Application.Commands;

public record AlterarDisponibilidadeCommand(Guid Id, bool Disponivel) : IRequest<bool>;

public class AlterarDisponibilidadeCommandHandler : IRequestHandler<AlterarDisponibilidadeCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publish;

    public AlterarDisponibilidadeCommandHandler(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IPublishEndpoint publish, ICategoriaRepository categoriaRepository)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
        _publish = publish;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<bool> Handle(AlterarDisponibilidadeCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto is null) return false;

        var categoria = await _categoriaRepository.ObterPorIdAsync(produto.CategoriaId);
        if (produto is null) return false;

        if (request.Disponivel)
            produto.TornarDisponivel();
        else
            produto.TornarIndisponivel();

        await _produtoRepository.AtualizarAsync(produto);
        await _unitOfWork.CommitAsync();

        await _publish.Publish<IProdutoAtualizadoEvent>(new ProdutoAtualizadoEvent
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Categoria = categoria!.Nome,
            Preco = produto.Preco,
            Disponibilidade = produto.Disponivel
        });

        return true;
    }
}