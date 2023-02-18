using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ProfileViewModel
    {
        public EditProfileViewModel EditProfileViewModel { get; set; }
        public  List<DoctorAppointment> DoctorAppointments { get; set; } = new List<DoctorAppointment>();
    }
}
