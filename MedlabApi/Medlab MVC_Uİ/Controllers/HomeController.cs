using Medlab.Core.Entities;
using Medlab_MVC_Uİ.Models;
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

        public HomeController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

      
       public IActionResult Index()
        {
            return View();
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