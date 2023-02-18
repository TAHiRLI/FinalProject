using FluentValidation;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class SetAppointmentViewModel
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
    }

    public class SetAppointmentVmValidator : AbstractValidator<SetAppointmentViewModel>
    {
        public SetAppointmentVmValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty(); 
            RuleFor(x => x.Date).NotEmpty().NotNull();
            RuleFor(x => x.Time).NotEmpty().NotNull();
        }
    }
}
