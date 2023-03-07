using FluentValidation;

namespace MedlabApi.Dtos.MessageDto
{
    public class MessageReplyDto
    {
        public string Subject { get; set; }
        public string Text { get; set; }
    }
    public class MessageReplyDtoValidator : AbstractValidator<MessageReplyDto>
    {
        public MessageReplyDtoValidator()
        {
            RuleFor(x => x.Subject).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Text).NotNull().NotEmpty().MaximumLength(500);
        }
    }
}
