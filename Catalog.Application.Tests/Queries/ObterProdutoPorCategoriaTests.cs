using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Moq;

namespace Catalog.Application.Tests.Queries;
public class ObterProdutoPorCategoriaTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;

    private readonly ObterProdutosPorCategoriaHandler _sut;

    private Produto ProdutoValido = new Produto("Produto", "Descricao", 10.0m, Guid.NewGuid());

    public ObterProdutoPorCategoriaTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _sut = new ObterProdutosPorCategoriaHandler(_repositoryMock.Object, _categoriaRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeverRetornarProdutos()
    {
        var query = new ObterProdutosPorCategoriaQuery("Categoria");

        _categoriaRepositoryMock
            .Setup(x => x.ObterPorNomeAsync(It.IsAny<string>()))
            .ReturnsAsync(new Categoria("Categoria"));

        _repositoryMock
            .Setup(x => x.ObterPorCategoriaAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Produto> { ProdutoValido });
        
        var result = await _sut.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(x => x.ObterPorCategoriaAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoriaNaoEncontrada_DeverRetornarListaVazia()
    {
        var query = new ObterProdutosPorCategoriaQuery("Categoria");

        _categoriaRepositoryMock
            .Setup(x => x.ObterPorNomeAsync(It.IsAny<string>()))
            .ReturnsAsync(default(Categoria?));

        var result = await _sut.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(x => x.ObterPorCategoriaAsync(It.IsAny<Guid>()), Times.Never);

        Assert.Empty(result);
    }
}
