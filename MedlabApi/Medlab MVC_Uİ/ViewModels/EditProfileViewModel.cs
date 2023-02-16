using FluentValidation;
using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class EditProfileViewModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Fullname { get; set; }
        public string UserName { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? NewPassword { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
    public class EditProfileVmValidator : AbstractValidator<EditProfileViewModel>
    {
        public EditProfileVmValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().MaximumLength(20);
            RuleFor(x => x.ImageUrl).MaximumLength(200);
            RuleFor(x => x.Fullname).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.UserName).NotEmpty().NotNull().MaximumLength(30);
            RuleFor(x => x.Password).MinimumLength(8).MaximumLength(20);
            RuleFor(x => x.NewPassword).MinimumLength(8).MaximumLength(20);
            RuleFor(x => x.ConfirmPassword).MinimumLength(8).MaximumLength(20).Equal(x => x.NewPassword);

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.ImageFile != null)
                {
                    if (x.ImageFile?.ContentType != "image/png" && x.ImageFile?.ContentType != "image/jpeg")
                    {
                        context.AddFailure("ImageFile", "File Type must be jpeg or png");
                    }
                    else if (x.ImageFile?.Length > 2097152)
                    {
                        context.AddFailure("ImageFile", "File size must be less than 2mb");
                    }

                }

            });
        }
    }
}
