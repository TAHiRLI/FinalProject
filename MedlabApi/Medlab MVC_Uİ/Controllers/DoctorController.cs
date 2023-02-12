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

        public DoctorController(IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository)
        {
            this._doctorRepository = doctorRepository;
            this._departmentRepository = departmentRepository;
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
        public IActionResult Details()
        {
            return View();

        }
    }
}
