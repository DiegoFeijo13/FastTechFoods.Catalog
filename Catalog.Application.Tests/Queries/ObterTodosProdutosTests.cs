using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Moq;

namespace Catalog.Application.Tests.Queries;
public class ObterTodosProdutosTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;    

    private readonly ObterTodosProdutosHandler _sut;

    private Produto ProdutoValido = new Produto("Produto", "Descricao", 10.0m, Guid.NewGuid());

    public ObterTodosProdutosTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        
        _sut = new ObterTodosProdutosHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_DeverRetornarProdutos()
    {
        var query = new ObterTodosProdutosQuery();

        _repositoryMock
            .Setup(x => x.ObterTodosAsync())
            .ReturnsAsync(new List<Produto> { ProdutoValido });
        
        var result = await _sut.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(x => x.ObterTodosAsync(), Times.Once);
    }
}
