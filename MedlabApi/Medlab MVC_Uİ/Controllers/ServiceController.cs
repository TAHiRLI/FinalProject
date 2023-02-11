using Microsoft.AspNetCore.Mvc;

namespace Medlab_MVC_Uİ.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
