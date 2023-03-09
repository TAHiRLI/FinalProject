using FluentValidation;

namespace MedlabApi.Dtos.BlgoCategoriyDtos
{
    public class BlogCategoryPutDto
    {
        public string Name { get; set; }
    }
    public class BlogCategoryPutDtoValidator : AbstractValidator<BlogCategoryPutDto>
    {
        public BlogCategoryPutDtoValidator()
        {
            RuleFor(b => b.Name).NotEmpty().NotNull().MaximumLength(30);
        }
    }
}
