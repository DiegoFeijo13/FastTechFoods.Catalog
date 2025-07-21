using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MassTransit;
using Moq;

namespace Catalog.Application.Tests.Commands;

public class AtualizarProdutoCommandHandlerTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPublishEndpoint> _publishMock;

    private readonly AtualizarProdutoCommandHandler _sut;

    private ProdutoInputDTO ProdutoInputDTO = new ProdutoInputDTO("Nome", "Descricao", 10.0m, "Categoria");
    private Produto ProdutoValido = new Produto("Produto", "Descricao", 10.0m, Guid.NewGuid());

    public AtualizarProdutoCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _publishMock = new Mock<IPublishEndpoint>();
        _sut = new AtualizarProdutoCommandHandler(
            _repositoryMock.Object,
            _categoriaRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _publishMock.Object
            );
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeveAtualizarProduto()
    {
        var guid = Guid.NewGuid();
        var command = new AtualizarProdutoCommand(guid, ProdutoInputDTO);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(ProdutoValido);

        _categoriaRepositoryMock
            .Setup(x => x.ObterTodosAsync())
            .ReturnsAsync(new List<Categoria> { new Categoria("Categoria") });            

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Produto>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InformadoDadosInvalidos_DeveRetornarFalse()
    {
        var guid = Guid.NewGuid();
        var command = new AtualizarProdutoCommand(guid, ProdutoInputDTO);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Produto?));

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Produto>()), Times.Never);

        Assert.False(result);
    }
}