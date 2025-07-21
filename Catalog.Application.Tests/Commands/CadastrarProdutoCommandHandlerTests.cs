using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MassTransit;
using Moq;

namespace Catalog.Application.Tests.Commands;

public class CadastrarProdutoCommandHandlerTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPublishEndpoint> _publishMock;

    private readonly CadastrarProdutoCommandHandler _sut;

    private ProdutoInputDTO ProdutoInputDTO = new ProdutoInputDTO("Nome", "Descricao", 10.0m, "Categoria");    

    public CadastrarProdutoCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _publishMock = new Mock<IPublishEndpoint>();
        _sut = new CadastrarProdutoCommandHandler(
            _repositoryMock.Object,
            _categoriaRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _publishMock.Object
            );
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeveCadastrarProduto()
    {        
        var command = new CadastrarProdutoCommand(ProdutoInputDTO);

        _categoriaRepositoryMock
            .Setup(x => x.ObterPorNomeAsync(It.IsAny<string>()))
            .ReturnsAsync(new Categoria("Categoria") { Id = Guid.NewGuid()});            

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoriaJaExiste_DeveCriarProdutoSemNovaCategoria()
    {
        // Arrange
        var dto = new ProdutoInputDTO("Coca-Cola", "Refrigerante", 10.0m, "Bebida");
        var categoriaExistente = new Categoria("Bebida");

        _categoriaRepositoryMock.Setup(r => r.ObterPorNomeAsync("Bebida"))
            .ReturnsAsync(categoriaExistente);

        var command = new CadastrarProdutoCommand(dto);

        // Act
        var resultado = await _sut.Handle(command, default);

        // Assert
        Assert.True(resultado);
        _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
        _categoriaRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoriaNaoExiste_DeveCriarCategoriaENovoProduto()
    {
        // Arrange
        var dto = new ProdutoInputDTO("Pepsi", "Refrigerante", 9.0m, "BebidaNova");

        _categoriaRepositoryMock.Setup(r => r.ObterPorNomeAsync("BebidaNova"))
            .ReturnsAsync((Categoria?)null);

        var command = new CadastrarProdutoCommand(dto);

        // Act
        var resultado = await _sut.Handle(command, default);

        // Assert
        Assert.True(resultado);
        _categoriaRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Once);
        _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}