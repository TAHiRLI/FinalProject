using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Medlab_MVC_Uİ.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IBlogRepostiory _blogRepostiory;

        public DoctorController(IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository, IBlogRepostiory blogRepostiory)
        {
            this._doctorRepository = doctorRepository;
            this._departmentRepository = departmentRepository;
            this._blogRepostiory = blogRepostiory;
        }
        public IActionResult Index(int? departmentId = null)
        {
            DoctorsViewModel model = new DoctorsViewModel();

            if (departmentId != null && !_departmentRepository.Any(x=> x.Id == departmentId))
            {
                return NotFound();
            }
             if(departmentId== null)
            {
                model.Doctors = _doctorRepository.GetAll(x => true).ToList();

            }
            else
            {
                model.Doctors = _doctorRepository.GetAll(x => x.DepartmentId == departmentId).ToList();
            }
            model.Deparments = _departmentRepository.GetAll(x => true).ToList();


            ViewBag.departmentId = departmentId;
            return View(model);
        }
        public async Task<IActionResult>  Details(int id)
        {
            Doctor doctor =await _doctorRepository.GetAsync(x => x.Id == id, "Blogs");
            if (doctor == null)
                return NotFound();

            DoctorDetailsViewModel model = new DoctorDetailsViewModel
            {
                Doctor = doctor,
                Doctors = _doctorRepository.GetAll(x => x.DepartmentId == doctor.DepartmentId).ToList(),
                Blogs = doctor.Blogs.OrderByDescending(x => x.CreatedAt).Take(2).ToList()
            };

            return View(model);

        }
    }
}
