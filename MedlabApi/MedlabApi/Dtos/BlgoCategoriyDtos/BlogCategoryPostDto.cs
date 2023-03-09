using FluentValidation;

namespace MedlabApi.Dtos.BlgoCategoriyDtos
{
    public class BlogCategoryPostDto
    {
        public string Name { get; set; }
    }
    public class BlogCategoryPostDtoValidator : AbstractValidator<BlogCategoryPostDto>
    {
        public BlogCategoryPostDtoValidator()
        {
            RuleFor(b => b.Name).NotEmpty().NotNull().MaximumLength(30);
        }
    }

}
