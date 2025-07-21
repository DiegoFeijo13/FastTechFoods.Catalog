using Catalog.Application.DTOs;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Moq;

namespace Catalog.Application.Tests.Queries;
public class ObterProdutoPorIdQueryTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;    

    private readonly ObterProdutoPorIdHandler _sut;

    private Produto ProdutoValido = new Produto("Produto", "Descricao", 10.0m, Guid.NewGuid());

    public ObterProdutoPorIdQueryTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();        
        _sut = new ObterProdutoPorIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeverRetornarProduto()
    {
        var guid = Guid.NewGuid();
        var query = new ObterProdutoPorIdQuery(guid);
        
        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(ProdutoValido);
        
        var result = await _sut.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(x => x.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_IdNaoEncontrado_DeverRetornarNull()
    {
        var guid = Guid.NewGuid();
        var query = new ObterProdutoPorIdQuery(guid);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Produto?));

        var result = await _sut.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(x => x.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);

        Assert.Null(result);
    }
}
