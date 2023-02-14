using FluentValidation;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class LoginVmValidator : AbstractValidator<LoginViewModel>
    {
        public LoginVmValidator()
        {
            RuleFor(x=> x.Username).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x=> x.Password).NotEmpty().NotNull().MaximumLength(20);

        }
    }
}
