using FluentValidation;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class OrderViewModel
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Note { get; set; }
    }
    public class OrderVmValidator : AbstractValidator<OrderViewModel>
    {
        public OrderVmValidator()
        {
            RuleFor(x => x.Fullname).NotNull().NotEmpty()
                .MaximumLength(30);
            RuleFor(x => x.Email).NotNull().NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty()
                .MaximumLength(20);
            RuleFor(x => x.ZipCode).NotNull().NotEmpty()
                .MaximumLength(10);
            RuleFor(x => x.Address1).NotNull().NotEmpty()
                .MaximumLength(200);
            RuleFor(x => x.Address2).MaximumLength(200);
            RuleFor(x => x.Note).MaximumLength(500);

        }
    }
}
