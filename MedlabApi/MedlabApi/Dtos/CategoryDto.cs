using FluentValidation;

namespace MedlabApi.Dtos
{
    public class CategoryDto
    {
        public string Name { get; set; }
    }
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("tahir, should not be empty").MaximumLength(20);
        }
    }

}
