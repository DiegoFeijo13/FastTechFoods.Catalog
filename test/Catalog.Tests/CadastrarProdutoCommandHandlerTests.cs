using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;
using Catalog.Domain.Repositories;
using MassTransit;
using Moq;

namespace Catalog.Tests;
public class CadastrarProdutoCommandHandlerTests
{
    private readonly Mock<IProdutoRepository> _produtoRepoMock = new();
    private readonly Mock<ICategoriaRepository> _categoriaRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPublishEndpoint> _publishMock = new();

    private readonly CadastrarProdutoCommandHandler _handler;

    public CadastrarProdutoCommandHandlerTests()
    {
        _handler = new CadastrarProdutoCommandHandler(
            _produtoRepoMock.Object,
            _categoriaRepoMock.Object,
            _unitOfWorkMock.Object,
            _publishMock.Object
        );
    }

    [Fact]
    public async Task Handle_CategoriaJaExiste_DeveCriarProdutoSemNovaCategoria()
    {
        // Arrange
        var dto = new ProdutoInputDTO("Coca-Cola", "Refrigerante", 10.0m, "Bebida");
        var categoriaExistente = new Categoria("Bebida");

        _categoriaRepoMock.Setup(r => r.ObterPorNomeAsync("Bebida"))
            .ReturnsAsync(categoriaExistente);

        var command = new CadastrarProdutoCommand(dto);

        // Act
        var resultado = await _handler.Handle(command, default);

        // Assert
        Assert.True(resultado);
        _produtoRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
        _categoriaRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoriaNaoExiste_DeveCriarCategoriaENovoProduto()
    {
        // Arrange
        var dto = new ProdutoInputDTO("Pepsi", "Refrigerante", 9.0m, "BebidaNova");

        _categoriaRepoMock.Setup(r => r.ObterPorNomeAsync("BebidaNova"))
            .ReturnsAsync((Categoria?)null);

        var command = new CadastrarProdutoCommand(dto);

        // Act
        var resultado = await _handler.Handle(command, default);

        // Assert
        Assert.True(resultado);
        _categoriaRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Once);
        _produtoRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}

