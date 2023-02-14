using FluentValidation;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class RegisterVmValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterVmValidator()
        {
            RuleFor(x=> x.Email).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x=> x.Fullname).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().MaximumLength(20);
            RuleFor(x => x.Username).NotEmpty().NotNull().MaximumLength(30);
            RuleFor(x => x.Password).NotEmpty().NotNull().MaximumLength(20).MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull().MaximumLength(20).MinimumLength(8).Equal(x => x.Password);
        }
    }
}
