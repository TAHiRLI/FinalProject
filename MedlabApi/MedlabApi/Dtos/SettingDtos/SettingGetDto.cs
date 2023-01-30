using FluentValidation;

namespace MedlabApi.Dtos.SettingDtos
{
    public class SettingGetDto
    {
        public int Id { get; set; }
        public string Key { get; set; }    
        public string Value { get; set; }
    }
    public class SettingGetDtoValidator : AbstractValidator<SettingGetDto>
    {
        public SettingGetDtoValidator()
        {
            RuleFor(x => x.Key).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(x=> x.Value).NotNull().NotEmpty().MaximumLength(1000);
        }
    }
}
