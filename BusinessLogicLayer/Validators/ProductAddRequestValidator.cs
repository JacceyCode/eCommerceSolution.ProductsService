using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator() {
        // ProductName
        RuleFor(product => product.ProductName)
            .NotEmpty().WithMessage("Product name is required.");

        // Category
         RuleFor(product => product.Category)
            .IsInEnum().WithMessage("Category is of defined options.");

        // UnitPrice
        RuleFor(product => product.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage($"Unit price should be between 0 and {double.MaxValue}");

        // QuantityInStock
        RuleFor(product => product.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity in stock should be between 0 and {int.MaxValue}");
    }
}
