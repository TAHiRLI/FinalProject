using FluentValidation;

namespace MedlabApi.Dtos.ServiceDtos
{

    public class ServicePutDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DetailedDesc { get; set; }
        public string Icon { get; set; }
        public bool isFeatured { get; set; }
        public IFormFile? Image { get; set; }
    }
    public class ServicePutDtoDtoValidator : AbstractValidator<ServicePutDto>
    {
        public ServicePutDtoDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(500);
            RuleFor(x => x.DetailedDesc).NotEmpty().NotNull().MaximumLength(1000);
            RuleFor(x => x.Icon).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Image != null)
                {
                    if (x.Image?.ContentType != "image/png" && x.Image?.ContentType != "image/jpeg")
                    {
                        context.AddFailure("Image", "File Type must be jpeg or png");
                    }
                    else if (x.Image?.Length > 2097152)
                    {
                        context.AddFailure("Image", "File size must be less than 2mb");
                    }
                }

            });
        }
    }
}
