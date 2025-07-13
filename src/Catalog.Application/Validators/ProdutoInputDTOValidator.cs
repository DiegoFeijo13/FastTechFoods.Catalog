using Catalog.Application.DTOs;
using FluentValidation;

namespace Catalog.Application.Validators;
public class ProdutoInputDTOValidator : AbstractValidator<ProdutoInputDTO>
{
    public ProdutoInputDTOValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage(ResourceExceptionMessages.NAME_EMPTY)
            .MaximumLength(100);

        RuleFor(p => p.Descricao)
            .NotEmpty().WithMessage(ResourceExceptionMessages.DESCRIPTION_EMPTY)
            .MaximumLength(500);

        RuleFor(p => p.Preco)
            .GreaterThan(0).WithMessage(ResourceExceptionMessages.PRICE_INVALID);

        RuleFor(p => p.Categoria)
            .NotEmpty().WithMessage(ResourceExceptionMessages.CATEGORY_INVALID)
            .MaximumLength(100);
    }
}

