using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Medlab_MVC_Uİ.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            this._serviceRepository = serviceRepository;
        }
        public IActionResult Index()
        {
            List<Service> model = _serviceRepository.GetAll(x => true).ToList();
            return View(model);
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
