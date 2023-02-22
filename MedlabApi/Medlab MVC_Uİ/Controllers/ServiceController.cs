using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.Helpers;
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
        public IActionResult Index(int? page = 1,int pageSize = 6)
        {
            List<Service> Services = _serviceRepository.GetAll(x => true).ToList();

            Pagination<Service> paginatedList = new Pagination<Service>();
            ViewBag.Services = paginatedList.GetPagedNames(Services, page, pageSize);
            ViewBag.SelectedPageSize = pageSize;



            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
