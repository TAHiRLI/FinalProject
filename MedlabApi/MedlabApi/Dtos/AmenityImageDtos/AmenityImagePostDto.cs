using FluentValidation;

namespace MedlabApi.Dtos.AmenityImageDtos
{
    public class AmenityImagePostDto
    {
        public IFormFile Image { get; set; }
    }
    public class AmenityImagePostDtoValidator : AbstractValidator<AmenityImagePostDto>
    {
        public AmenityImagePostDtoValidator()
        {
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Image == null)
                {
                    context.AddFailure("Image", "Image Is Required");
                }
                else if (x.Image?.ContentType != "image/png" && x.Image?.ContentType != "image/jpeg")
                {
                    context.AddFailure("Image", "File Type must be jpeg or png");
                }
                else if (x.Image?.Length > 2097152)
                {
                    context.AddFailure("Image", "File size must be less than 2mb");
                }

            });
        }
    }
}