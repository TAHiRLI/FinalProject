using FluentValidation;

namespace MedlabApi.Dtos.ProductCategoryDtos
{
    public class ProductCategoryPostDto
    {
        public string Name { get; set; }
    }
    public class ProductCategoryPostDtoValidator : AbstractValidator<ProductCategoryPostDto>
    {
        public ProductCategoryPostDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().MaximumLength(30);
        }
    }
}
