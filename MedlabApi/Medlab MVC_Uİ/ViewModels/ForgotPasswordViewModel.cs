using FluentValidation;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordVmValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordVmValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(50);
        }
    }
}
