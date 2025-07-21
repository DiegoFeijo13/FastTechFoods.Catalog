using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MassTransit;
using Moq;

namespace Catalog.Application.Tests.Commands;

public class RemoverProdutoCommandHandlerTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;    
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPublishEndpoint> _publishMock;

    private readonly RemoverProdutoCommandHandler _sut;

    private Produto ProdutoValido = new Produto("Produto", "Descricao", 10.0m, Guid.NewGuid());

    public RemoverProdutoCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();        
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _publishMock = new Mock<IPublishEndpoint>();
        _sut = new RemoverProdutoCommandHandler(
            _repositoryMock.Object,            
            _unitOfWorkMock.Object,
            _publishMock.Object
            );
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeveRemoverProduto()
    {
        var guid = Guid.NewGuid();
        var command = new RemoverProdutoCommand(guid);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(ProdutoValido);

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.RemoverAsync(It.IsAny<Produto>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InformadoDadosInvalidos_DeveRetornarFalse()
    {
        var guid = Guid.NewGuid();
        var command = new RemoverProdutoCommand(guid);

        _repositoryMock
            .Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Produto?));

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.RemoverAsync(It.IsAny<Produto>()), Times.Never);

        Assert.False(result);
    }
}