using Microsoft.AspNetCore.Mvc;

namespace Medlab_MVC_Uİ.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();

        }
    }
}
