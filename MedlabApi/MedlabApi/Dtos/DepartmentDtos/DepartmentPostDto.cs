using FluentValidation;
using MedlabApi.Dtos.ProductCategoryDtos;

namespace MedlabApi.Dtos.DepartmentDtos
{
    public class DepartmentPostDto
    {
        public string Name { get; set; }
    }
    public class DepartmentPostDtoValidator : AbstractValidator<DepartmentPostDto>
    {
        public DepartmentPostDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().MaximumLength(30);
        }
    }
}
