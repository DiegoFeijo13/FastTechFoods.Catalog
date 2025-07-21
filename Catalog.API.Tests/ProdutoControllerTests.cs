using Catalog.API.Controllers;
using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Catalog.API.Tests;

public class ProdutoControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IValidator<ProdutoInputDTO>> _validatorMock;
    private readonly ProdutoController _sut;

    private ValidationResult _validationResult;

    private static ProdutoInputDTO ProdutoValido => new("Produto", "Descricao", 10.0m, "Categoria");
    private static ProdutoInputDTO ProdutoInvalido => new("", "", 0.0m, "");

    private static ProdutoOutputDTO ProdutoOutput => new("Produto", "Descricao", 10.0m);

    public ProdutoControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new Mock<IValidator<ProdutoInputDTO>>();

        _sut = new(_mediatorMock.Object);

        _validationResult = new() { };

        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ProdutoInputDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => _validationResult);
    }

    #region Cadastrar
    [Fact]
    public async Task Cadastrar_InformadosDadosValidos_DeveRetornarOkResult()
    {
        var command = new CadastrarProdutoCommand(ProdutoValido);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CadastrarProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.Cadastrar(ProdutoValido, _validatorMock.Object);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Cadastrar_Erro_DeveRetornarBadRequest()
    {
        var command = new CadastrarProdutoCommand(ProdutoInvalido);

        _validationResult = new() { Errors = new()
            {
                new ValidationFailure { ErrorMessage = ""}
            }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CadastrarProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.Cadastrar(ProdutoInvalido, _validatorMock.Object);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Cadastrar_DadosInvalidos_DeveRetornarBadRequest()
    {
        var command = new CadastrarProdutoCommand(ProdutoInvalido);

        _validationResult = new() { };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CadastrarProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.Cadastrar(ProdutoInvalido, _validatorMock.Object);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    #endregion Cadastrar

    #region Atualizar
    [Fact]
    public async Task Atualizar_InformadosDadosValidos_DeveRetornarOk()
    {
        var guid = Guid.NewGuid();        

        var command = new AtualizarProdutoCommand(guid, ProdutoValido);

        _validationResult = new() { };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.Atualizar(guid, ProdutoValido, _validatorMock.Object);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Atualizar_Erro_DeveRetornarBadRequest()
    {
        var guid = Guid.NewGuid();        

        var command = new AtualizarProdutoCommand(guid, ProdutoValido);

        _validationResult = new();

        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.Atualizar(guid, ProdutoValido, _validatorMock.Object);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Atualizar_DadosInvalidos_DeveRetornarBadRequest()
    {
        var guid = Guid.NewGuid();        

        var command = new AtualizarProdutoCommand(guid, ProdutoInvalido);

        _validationResult = new()
        {
            Errors = new()
            {
                new ValidationFailure { ErrorMessage = ""}
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.Atualizar(guid, ProdutoInvalido, _validatorMock.Object);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    #endregion Atualizar

    #region Delete
    [Fact]
    public async Task Delete_InformadosDadosValidos_DeveRetornarOk()
    {
        var guid = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.Delete(guid);
        Assert.IsType<OkObjectResult>(result);        
    }

    [Fact]
    public async Task Delete_DadosInvalidos_DeveRetornarNotFound()
    {
        var guid = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverProdutoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.Delete(guid);
        Assert.IsType<NotFoundObjectResult>(result);
    }
    #endregion Delete

    #region AlterarDisponibilidade
    [Fact]
    public async Task AlterarDisponibilidade_InformadosDadosValidos_DeveRetornarOk()
    {
        var guid = Guid.NewGuid();
        var disponivel = true;

        var command = new AlterarDisponibilidadeCommand(guid, disponivel);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AlterarDisponibilidadeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.AlterarDisponibilidade(guid, disponivel);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task AlterarDisponibilidade_Erro_DeveRetornarNotFound()
    {
        var guid = Guid.NewGuid();
        var disponivel = true;

        var command = new AlterarDisponibilidadeCommand(guid, disponivel);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AlterarDisponibilidadeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.AlterarDisponibilidade(guid, disponivel);

        Assert.IsType<NotFoundObjectResult>(result);
    }
    #endregion AlterarDisponibilidade

    #region GetById
    [Fact]
    public async Task GetById_InformadosDadosValidos_DeveRetornarProduto()
    {
        var guid = Guid.NewGuid();        

        var query = new ObterProdutoPorIdQuery(guid);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ObterProdutoPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ProdutoOutput);

        var result = await _sut.GetById(guid);

        Assert.IsType<ActionResult<Produto>>(result);
    }

    [Fact]
    public async Task GetById_InformadosDadosInvalidos_DeveRetornarNotFound()
    {
        var guid = Guid.NewGuid();

        var query = new ObterProdutoPorIdQuery(guid);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ObterProdutoPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(ProdutoOutputDTO?));

        var result = await _sut.GetById(guid);

        var resultObject = Assert.IsType<ActionResult<Produto>>(result);

        Assert.IsType<NotFoundResult>(resultObject.Result);
    }
    #endregion GetById

    #region GetAll
    [Fact]
    public async Task GetAll_DeveRetornarProdutos()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ObterTodosProdutosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProdutoOutputDTO> { ProdutoOutput });

        var result = await _sut.GetAll();

        Assert.IsType<ActionResult<IEnumerable<Produto>>>(result);
    }
    #endregion GetAll

    #region Buscar
    [Fact]
    public async Task Buscar_DeveRetornarProdutos()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BuscarProdutoPorTermoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Produto>());

        var result = await _sut.Buscar(string.Empty);

        Assert.IsType<ActionResult<IEnumerable<Produto>>>(result);
    }
    #endregion Buscar

    #region GetByCategoria
    [Fact]
    public async Task GetByCategoria_DeveRetornarProdutos()
    {
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ObterProdutosPorCategoriaQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProdutoOutputDTO>());

        var result = await _sut.GetByCategoria(string.Empty);

        Assert.IsType<ActionResult<IEnumerable<Produto>>>(result);
    }
    #endregion GetByCategoria
}