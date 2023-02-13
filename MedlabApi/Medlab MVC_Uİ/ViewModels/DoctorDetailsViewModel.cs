using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class DoctorDetailsViewModel
    {
        public Doctor Doctor { get; set; }
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public List<Blog> Blogs { get; set; } = new List<Blog>();   
    }
}
