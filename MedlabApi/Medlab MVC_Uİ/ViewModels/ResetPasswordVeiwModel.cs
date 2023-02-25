using FluentValidation;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ResetPasswordVeiwModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
    public class ResetPasswordVmValidator : AbstractValidator<ResetPasswordVeiwModel>
    {
        public ResetPasswordVmValidator()
        {
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8).MaximumLength(20);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        }
    }
}
