using FluentValidation;

namespace MedlabApi.Dtos.SettingDtos
{
    public class SettingPutDto
    {
        public string Value { get; set; }
    }
    public class SettingPutsDtoValidator : AbstractValidator<SettingPutDto>
    {
        public SettingPutsDtoValidator()
        {
            RuleFor(x => x.Value).NotEmpty().MaximumLength(1000);
        }
    }
}
