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
        private readonly IMapper _mapper;
        private readonly IBlogRepostiory _blogRepostiory;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValueRepository _valueRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ISubscriptionRepostiory _subscriptionRepostiory;
        private readonly IAmenityImageRepository _amenityImageRepository;
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IDoctorAppointmentRepository _doctorAppointmentRepository;

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
            ISubscriptionRepostiory subscriptionRepostiory,
            IAmenityImageRepository amenityImageRepository,
            IContactMessageRepository contactMessageRepository,
            IDoctorAppointmentRepository doctorAppointmentRepository
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _blogRepostiory = blogRepostiory;
            _valueRepository = valueRepository;
            _sliderRepository = sliderRepository;
            _doctorRepository = doctorRepository;
            _settingRepository = settingRepository;
            _serviceRepository = serviceRepository;
            _subscriptionRepostiory = subscriptionRepostiory;
            _amenityImageRepository = amenityImageRepository;
            _contactMessageRepository = contactMessageRepository;
            _doctorAppointmentRepository = doctorAppointmentRepository;
        }

        //===============================
        // Index
        //===============================

        public async Task<IActionResult> Index()
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
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return NotFound();
                model.ContactMesssageVm.Email = user.Email;
                model.ContactMesssageVm.FullName = user.Fullname;
                model.ContactMesssageVm.PhoneNumber = user.PhoneNumber;

            }
            return View(model);
        }

        //===============================
        // About Us
        //===============================
        public IActionResult AboutUs()
        {
            AboutUsViewModel model = new AboutUsViewModel
            {
                Values = _valueRepository.GetAll(x => true).Take(20).ToList(),
                AmenityImages = _amenityImageRepository.GetAll(x => true).ToList()
            };
            return View(model);
        }
        //===============================
        // Contact
        //===============================

        public async Task<IActionResult> Contact()
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

        // for Home Page

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // for Contact Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendContactRequest(ContactMesssageViewModel MessageVm)
        {
            if (!ModelState.IsValid)
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

                return View("contact", model);
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


            return RedirectToAction("contact");
        }

        //============================
        // Subscribe
        //============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string Email)
        {
            if (Email.Length > 50)
            {
                TempData["ErrorMessage"] = "Maximum Length is 50 characters";
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
                return RedirectToAction("index", model);
            }
            Subscription subscription = new Subscription { Email = Email };
            await _subscriptionRepostiory.AddAsync(subscription);
            await _subscriptionRepostiory.CommitAsync();

            return RedirectToAction("index");
        }


        //===============================
        // Error page
        //===============================

        public IActionResult Error()
        {
            return View();
        }

    }
}