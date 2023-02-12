using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Medlab_MVC_Uİ.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISliderRepository _sliderRepository;

        public HomeController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ISliderRepository sliderRepository )
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._sliderRepository = sliderRepository;
        }


        public IActionResult Index()
        {
            homeViewModel model = new homeViewModel
            {
                Sliders = _sliderRepository.GetAll(x => true).OrderBy(x=>x.Order).ToList()
        };
            return View(model);
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
       
    }
}