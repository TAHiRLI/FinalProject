using FluentValidation;

namespace MedlabApi.Dtos.ValueDtos
{
    public class ValuePostDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Desc { get; set; }
        public bool IsFeatured { get; set; }
    }
    public class ValuePostDtoValidator : AbstractValidator<ValuePostDto>
    {
        public ValuePostDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Desc).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(x => x.Icon).NotNull().NotEmpty().MaximumLength(50);
        }
    }
}
