using FluentValidation;

namespace MedlabApi.Dtos.DoctorDtos
{
    public class DoctorPutDto
    {
        public int? DepartmentId { get; set; }
        public string Positon { get; set; }
        public string Office { get; set; }
        public string Fullname { get; set; }
        public string Desc { get; set; }
        public string DetailedDesc { get; set; }
        public decimal Salary { get; set; }
        public decimal MeetingPrice { get; set; }
        public bool Gender { get; set; }
        public bool IsFeatured { get; set; }
        public string Email { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public IFormFile? Image { get; set; }
    }
    public class DoctorPutDtoValidator : AbstractValidator<DoctorPutDto>
    {
        public DoctorPutDtoValidator()
        {

            RuleFor(x => x.Fullname).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Office).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Positon).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Desc).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(x => x.DetailedDesc).NotNull().NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Facebook).MaximumLength(100);
            RuleFor(x => x.Twitter).MaximumLength(100);
            RuleFor(x => x.Instagram).MaximumLength(100);
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
