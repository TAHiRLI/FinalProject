using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ProfileViewModel
    {
        public EditProfileViewModel EditProfileViewModel { get; set; }
        public  List<DoctorAppointment> DoctorAppointments { get; set; } = new List<DoctorAppointment>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public Doctor Doctor { get; set; }
        public string UserPhoto { get; set; }
        public string Fullname { get; set; }
    }
}
