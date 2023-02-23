using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class HomeViewModel
    {
        public Dictionary<string, string > Setting { get; set; }  =  new Dictionary<string, string>();
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Value> Values { get; set; } = new List<Value>();
        public List<Blog> RecentBlogs { get; set; } = new List<Blog>();
        public List<Service> Services { get; set; } = new List<Service>();
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public int DoctorCount { get; set; }
        public int ServiceCount { get; set; }
        public int AppointmentCount { get; set; }
        public ContactMesssageViewModel ContactMesssageVm { get; set; } = new ContactMesssageViewModel();
        
    }
}
