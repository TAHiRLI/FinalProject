using FluentValidation;
using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ProductReviewViewModel
    {
        public string Text { get; set; }
        public int Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? UserName { get; set; }
        public int ProductId { get; set; }
    }
    public class ProductReviewVmValidator : AbstractValidator<ProductReviewViewModel>
    {
        public ProductReviewVmValidator()
        {
            RuleFor(x => x.Text).NotEmpty().NotNull().MaximumLength(200);
            RuleFor(x => x.Rate).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
        }
    }
}
