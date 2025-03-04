using CatalogAPI.DTOs;
using FluentValidation;

namespace CatalogAPI.Validators
{
    public class CategoriaValidator : AbstractValidator<PostCategoriaDTO>
    {
        public CategoriaValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MinimumLength(3).WithMessage("O nome da categoria deve ter no mínimo 3 caracteres.");
        }
    }
}
