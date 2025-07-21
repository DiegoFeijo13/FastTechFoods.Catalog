using Catalog.Application.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MassTransit;
using Moq;

namespace Catalog.Application.Tests.Commands;

public class AlterarDisponibilidadeCommandHandlerTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPublishEndpoint> _publishMock;

    private readonly AlterarDisponibilidadeCommandHandler _sut;

    private Produto ProdutoValido = new Produto("Produto", "Descricao", 10.0m, Guid.NewGuid());

    public AlterarDisponibilidadeCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _publishMock = new Mock<IPublishEndpoint>();
        _sut = new AlterarDisponibilidadeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _publishMock.Object,
            _categoriaRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Disponivel_DeveAlterarStatus()
    {
        var guid = Guid.NewGuid();
        var command = new AlterarDisponibilidadeCommand(guid, true);


        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(ProdutoValido);

        _categoriaRepositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Categoria("Categoria"));            

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Produto>()), Times.Once);

        Assert.True(ProdutoValido.Disponivel);
    }

    [Fact]
    public async Task Handle_Indisponivel_DeveAlterarStatus()
    {
        var guid = Guid.NewGuid();
        var command = new AlterarDisponibilidadeCommand(guid, false);


        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(ProdutoValido);

        _categoriaRepositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Categoria("Categoria"));

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Produto>()), Times.Once);

        Assert.False(ProdutoValido.Disponivel);
    }
}