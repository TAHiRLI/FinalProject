using FluentValidation;

namespace MedlabApi.Dtos.ProductCategoryDtos
{
    public class ProductCategoryPutDto
    {
        public string Name { get; set; }
    }
    public class ProductCategoryPutDtoValidator : AbstractValidator<ProductCategoryPutDto>
    {
        public ProductCategoryPutDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().MaximumLength(30);
        }
    }
}
