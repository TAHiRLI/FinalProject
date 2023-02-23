using AutoMapper;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IDoctorAppointmentRepository _doctorAppointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IBlogRepostiory _blogRepostiory;
        private readonly IValueRepository _valueRepository;
        private readonly IAmenityImageRepository _amenityImageRepository;
        private readonly IContactMessageRepository _contactMessageRepository;

        public HomeController( 
            // Arguments
            IMapper mapper,
            IBlogRepostiory blogRepostiory, 
            IValueRepository valueRepository,
            UserManager<AppUser> userManager,
            IDoctorRepository doctorRepository,
            ISliderRepository sliderRepository,
            ISettingRepository settingRepository,
            IServiceRepository serviceRepository,
            IAmenityImageRepository amenityImageRepository,
            IContactMessageRepository contactMessageRepository,
            IDoctorAppointmentRepository doctorAppointmentRepository
            )
        {
            this._sliderRepository = sliderRepository;
            _userManager = userManager;
            _mapper = mapper;
            _doctorAppointmentRepository = doctorAppointmentRepository;
            _doctorRepository = doctorRepository;
            this._settingRepository = settingRepository;
            _serviceRepository = serviceRepository;
            _blogRepostiory = blogRepostiory;
            this._valueRepository = valueRepository;
            this._amenityImageRepository = amenityImageRepository;
            _contactMessageRepository = contactMessageRepository;
        }


        public async Task<IActionResult>  Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Setting = _settingRepository.GetSettingDictionary(),   
                Sliders = _sliderRepository.GetAll(x => true).OrderBy(x => x.Order).ToList(),
                Values = _valueRepository.GetAll(x => x.IsFeatured).ToList(),
                RecentBlogs = _blogRepostiory.GetAll(x => true).OrderByDescending(x => x.CreatedAt).Take(3).ToList(),
                Services = _serviceRepository.GetAll(x => x.isFeatured).ToList(),
                Doctors = _doctorRepository.GetAll(x => x.IsFeatured).ToList(),
                DoctorCount = _doctorRepository.GetAll(x => true).Count(),
                ServiceCount = _serviceRepository.GetAll(x => true).Count(),
                AppointmentCount = _doctorAppointmentRepository.GetAll(x => true).Count()
          

             };

            if (User.Identity.IsAuthenticated)
            {
                var user =await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return NotFound();
                model.ContactMesssageVm.Email = user.Email;
                model.ContactMesssageVm.FullName = user.Fullname;
                model.ContactMesssageVm.PhoneNumber = user.PhoneNumber;

            }
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
        public async Task<IActionResult>  Contact()
        {
            ContactViewModel model = new ContactViewModel
            {
                Setting = _settingRepository.GetSettingDictionary(),
            };
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return NotFound();
                model.ContactMesssageVm.Email = user.Email;
                model.ContactMesssageVm.FullName = user.Fullname;
                model.ContactMesssageVm.PhoneNumber = user.PhoneNumber;

            }

            return View(model);
        }
        public async Task<IActionResult> SendContactMessage(ContactMesssageViewModel MessageVm)
        {
            if (!ModelState.IsValid)
            {
                HomeViewModel model = new HomeViewModel
                {
                    Setting = _settingRepository.GetSettingDictionary(),
                    Sliders = _sliderRepository.GetAll(x => true).OrderBy(x => x.Order).ToList(),
                    Values = _valueRepository.GetAll(x => x.IsFeatured).ToList(),
                    RecentBlogs = _blogRepostiory.GetAll(x => true).OrderByDescending(x => x.CreatedAt).Take(3).ToList(),
                    Services = _serviceRepository.GetAll(x => x.isFeatured).ToList(),
                    Doctors = _doctorRepository.GetAll(x => x.IsFeatured).ToList(),
                    DoctorCount = _doctorRepository.GetAll(x => true).Count(),
                    ServiceCount = _serviceRepository.GetAll(x => true).Count(),
                    AppointmentCount = _doctorAppointmentRepository.GetAll(x => true).Count(),
                    ContactMesssageVm = MessageVm

                };

                return View("index", model);
            }

            ContactMessage contactMessage = _mapper.Map<ContactMessage>(MessageVm);

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return NotFound();
                contactMessage.AppUserId = user.Id;
            }

            await _contactMessageRepository.AddAsync(contactMessage);
            await _contactMessageRepository.CommitAsync();


            return RedirectToAction("Index");
        }
        public IActionResult Error()
        {
            return View();
        }
       
    }
}