using FluentValidation;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class BlogCreateViewModel
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
        public string MainContent { get; set; }
        public string PrevContent { get; set; }

    }
    public class BlogCreateVmValidator : AbstractValidator<BlogCreateViewModel>
    {
        public BlogCreateVmValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.CategoryId).NotNull();
            RuleFor(x => x.MainContent).NotNull().NotEmpty()
                .MaximumLength(3000);
            RuleFor(x => x.PrevContent).NotNull().NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.ImageFile == null)
                {
                    context.AddFailure("ImageFile", "Main Image Is Required");

                }
                else if (x.ImageFile?.ContentType != "image/png" && x.ImageFile?.ContentType != "image/jpeg")
                {
                    context.AddFailure("ImageFile", "File Type must be jpeg or png");
                }
                else if (x.ImageFile?.Length > 2097152)
                {
                    context.AddFailure("ImageFile", "File size must be less than 2mb");
                }



            });
        }
    }
}
