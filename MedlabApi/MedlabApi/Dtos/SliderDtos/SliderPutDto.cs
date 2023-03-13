using FluentValidation;

namespace MedlabApi.Dtos.SliderDtos
{
    public class SliderPutDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }

        public string BtnText { get; set; }
        public string BtnUrl { get; set; }
        public IFormFile? Image { get; set; }
        public int Order { get; set; }
    }

    public class SliderPutDtoValidator : AbstractValidator<SliderPutDto>
    {
        public SliderPutDtoValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Desc).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.BtnText).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.BtnUrl).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Image != null)
                {
                 if (x.Image?.ContentType != "image/png" && x.Image?.ContentType != "image/jpeg")
                {
                    context.AddFailure("Image", "File Type must be jpeg or png");
                }
                else if (x.Image?.Length > 40000000)
                {
                    context.AddFailure("Image", "File size must be less than 5mb");
                }

                }
            });

        }
    }
}
