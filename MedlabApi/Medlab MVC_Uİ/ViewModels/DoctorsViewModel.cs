using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class DoctorsViewModel
    {
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public List<Department> Deparments { get; set; } = new List<Department>();

    }
}
