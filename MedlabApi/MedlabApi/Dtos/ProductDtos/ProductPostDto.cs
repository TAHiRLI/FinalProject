using FluentValidation;

namespace MedlabApi.Dtos.ProductDtos
{
    public class ProductPostDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int? ProductCategoryId { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsSoldIndividual { get; set; }
        public bool StockStatus { get; set; }

        public IFormFile PosterImage { get; set; }
        public List<IFormFile>? OtherImages { get; set; }

    }
    public class ProductPostDtoValidator : AbstractValidator<ProductPostDto>
    {
        public ProductPostDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Desc).NotNull().NotEmpty().MaximumLength(300);
            RuleFor(x => x.ProductCategoryId).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CostPrice).NotNull().NotEmpty();
            RuleFor(x => x.SalePrice).NotNull().NotEmpty();
            RuleFor(x => x.DiscountPercent).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.PosterImage == null)
                {
                    context.AddFailure("PosterImage", "Main Image Is Required");
                }
                else if (x.PosterImage?.ContentType != "image/png" && x.PosterImage?.ContentType != "image/jpeg")
                {
                    context.AddFailure("PosterImage", "File Type must be jpeg or png");
                }
                else if (x.PosterImage?.Length > 2097152)
                {
                    context.AddFailure("PosterImage", "File size must be less than 2mb");
                }



            });
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.OtherImages != null && x.OtherImages.Count > 0)
                {
                    foreach (var image in x.OtherImages)
                    {
                        if (image?.ContentType != "image/png" && image?.ContentType != "image/jpeg")
                        {
                            context.AddFailure("OtherImages", "File Type must be jpeg or png");
                        }
                        else if (image?.Length > 2097152)
                        {
                            context.AddFailure("OtherImages", "File size must be less than 2mb");
                        }
                    }

                }
            });
        }
    }
}
