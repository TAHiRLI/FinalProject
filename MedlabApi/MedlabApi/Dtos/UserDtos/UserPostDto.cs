using FluentValidation;

namespace MedlabApi.Dtos.UserDtos
{
    public class UserPostDto
    {
        public string Username { get; set; }
        public IFormFile Image { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
    public class UserPostDtoValidator : AbstractValidator<UserPostDto>
    {
        public UserPostDtoValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Fullname).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(x => x.Role).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Image == null)
                {
                    context.AddFailure("Image", " Image Is Required");
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
