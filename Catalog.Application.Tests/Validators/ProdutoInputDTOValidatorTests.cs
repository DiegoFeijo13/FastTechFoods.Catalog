using Catalog.Application.DTOs;
using Catalog.Application.Validators;

namespace Catalog.Application.Tests.Validators;
public class ProdutoInputDTOValidatorTests
{
    private readonly ProdutoInputDTOValidator _validator = new();

    [Fact]
    public void ProdutoValido_DeveSerValido()
    {
        var dto = new ProdutoInputDTO("Coca-Cola", "Refrigerante sabor cola", 10.5m, "Bebida");

        var result = _validator.Validate(dto);

        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_DadoNomeVazio_DeveLancarErro()
    {
        var dto = new ProdutoInputDTO("", "descricao", 10, "categoria");

        var result = _validator.Validate(dto);

        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceExceptionMessages.NAME_EMPTY));
    }

    [Fact]
    public void Validate_DadoDescricaoVazia_DeveLancarErro()
    {
        var dto = new ProdutoInputDTO("nome", "", 10, "categoria");

        var result = _validator.Validate(dto);

        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceExceptionMessages.DESCRIPTION_EMPTY));
    }

    [Fact]
    public void Validate_DadoPrecoInvalido_DeveLancarErro()
    {
        var dto = new ProdutoInputDTO("nome", "descricao", 0, "categoria");

        var result = _validator.Validate(dto);

        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceExceptionMessages.PRICE_INVALID));
    }
    [Fact]
    public void Validate_DadoCategoriavazia_DeveLancarErro()
    {
        var dto = new ProdutoInputDTO("nome", "descricao", 0, "");

        var result = _validator.Validate(dto);

        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceExceptionMessages.CATEGORY_INVALID));
    }
}