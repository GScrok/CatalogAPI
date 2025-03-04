using CatalogAPI.DTOs;
using FluentValidation;

namespace CatalogAPI.Validators
{
    public class ProdutoValidator : AbstractValidator<PostProdutoDTO>
    {
        public ProdutoValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MinimumLength(3).WithMessage("O nome do produto deve ter no mínimo 3 caracteres.");

            RuleFor(p => p.Preco)
                .NotEmpty().WithMessage("O preço do produto é obrigatório.")
                .GreaterThan(0).WithMessage("O preço do produto deve ser maior que zero.");

            RuleFor(p => p.Estoque)
                .NotEmpty().WithMessage("A quantidade do produto é obrigatória.")
                .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo.");

            RuleFor(p => p.CategoriaId)
                .NotEmpty().WithMessage("O produto deve pertencer a uma categoria.");
        }
    }
}
