using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Moq;

namespace Catalog.Application.Tests.Queries;
public class BuscarProdutoPorTermoTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;

    private readonly BuscarProdutoPorTermoHandler _sut;

    private Produto ProdutoValido = new Produto("Produto", "Descricao", 10.0m, Guid.NewGuid());

    public BuscarProdutoPorTermoTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _sut = new BuscarProdutoPorTermoHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeverRetornarProdutos()
    {
        var query = new BuscarProdutoPorTermoQuery("produto");

        _repositoryMock
            .Setup(x => x.BuscarAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Produto> { ProdutoValido });
        
        var result = await _sut.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(x => x.BuscarAsync(query.Termo), Times.Once);
    }
}
