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
        private readonly ISliderRepository _sliderRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IValueRepository _valueRepository;
        private readonly IAmenityImageRepository _amenityImageRepository;

        public HomeController( ISliderRepository sliderRepository, ISettingRepository settingRepository, IValueRepository valueRepository, IAmenityImageRepository amenityImageRepository )
        {
            this._sliderRepository = sliderRepository;
            this._settingRepository = settingRepository;
            this._valueRepository = valueRepository;
            this._amenityImageRepository = amenityImageRepository;
        }


        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Sliders = _sliderRepository.GetAll(x => true).OrderBy(x=>x.Order).ToList()
        };
            return View(model);
        }
        public IActionResult AboutUs()
        {
            AboutUsViewModel model = new AboutUsViewModel
            {
                Values = _valueRepository.GetAll(x => true).Take(20).ToList(),
                AmenityImages = _amenityImageRepository.GetAll(x => true).ToList()
         };
            return View(model);
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