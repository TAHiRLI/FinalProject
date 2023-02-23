
using FluentValidation;
using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ContactMesssageViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class ContactMessageVmValidator : AbstractValidator<ContactMesssageViewModel>
    {
        public ContactMessageVmValidator()
        {
            RuleFor(x => x.FullName).NotNull().NotEmpty()
                .MaximumLength(30);
            RuleFor(x => x.Email).NotNull().NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty()
                .MaximumLength(20);
            RuleFor(x => x.Message).NotNull().NotEmpty()
                .MaximumLength(500);
        }
    }
}
