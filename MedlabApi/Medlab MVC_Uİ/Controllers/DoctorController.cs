using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Medlab_MVC_Uİ.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IBlogRepostiory _blogRepostiory;
        private readonly IDoctorAppointmentRepository _doctorAppointmentRepository;

        public DoctorController(IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository, IBlogRepostiory blogRepostiory, IDoctorAppointmentRepository doctorAppointmentRepository)
        {
            this._doctorRepository = doctorRepository;
            this._departmentRepository = departmentRepository;
            this._blogRepostiory = blogRepostiory;
            _doctorAppointmentRepository = doctorAppointmentRepository;
        }

        //======================
        // Index 
        //======================

        public IActionResult Index(int? departmentId = null)
        {
            DoctorsViewModel model = new DoctorsViewModel();

            if (departmentId != null && !_departmentRepository.Any(x => x.Id == departmentId))
            {
                return NotFound();
            }
            if (departmentId == null)
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

        //======================
        // Details 
        //======================

        public async Task<IActionResult> Details(int id)
        {
            Doctor doctor = await _doctorRepository.GetAsync(x => x.Id == id, "Blogs");
            if (doctor == null)
                return NotFound();

            DoctorDetailsViewModel model = new DoctorDetailsViewModel
            {
                Doctor = doctor,
                Doctors = _doctorRepository.GetAll(x => x.DepartmentId == doctor.DepartmentId).ToList(),
                Blogs = doctor.Blogs.OrderByDescending(x => x.CreatedAt).Take(2).ToList(),
                SetAppointmentVm = new SetAppointmentViewModel
                {
                    Date = DateTime.UtcNow.AddHours(4),
                    DoctorId = doctor.Id,
                }
            };

            return View(model);

        }


        //======================
        // Set Appointment 
        //======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> SetAppointment(SetAppointmentViewModel AppointmentVm)
        {
            var doctor = await _doctorRepository.GetAsync(x => x.Id == AppointmentVm.DoctorId, "DoctorAppointments");
            if (doctor == null)
                return NotFound();

            

            DoctorDetailsViewModel model = new DoctorDetailsViewModel
            {
                Doctor = doctor,
                Doctors = _doctorRepository.GetAll(x => x.DepartmentId == doctor.DepartmentId).ToList(),
                Blogs = doctor.Blogs.OrderByDescending(x => x.CreatedAt).Take(2).ToList(),
                SetAppointmentVm = new SetAppointmentViewModel
                {
                    Date = DateTime.UtcNow.AddHours(4),
                    DoctorId = doctor.Id,
                }
            };


            if (AppointmentVm.Date.Date + AppointmentVm.Time.TimeOfDay < DateTime.UtcNow.AddHours(4))
            {
                ModelState.AddModelError("Date", "Please Select Valid Date");
                ModelState.AddModelError("Time", "Please Select Valid Time");
            }

            if (!ModelState.IsValid)
                return View("Details", model);

            DoctorAppointment doctorAppointment = new DoctorAppointment
            {
                AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                DoctorId = AppointmentVm.DoctorId,
                IsApproved = null,
                MeetingDate = AppointmentVm.Date.Date + AppointmentVm.Time.TimeOfDay
            };

            await _doctorAppointmentRepository.AddAsync(doctorAppointment);
            _doctorAppointmentRepository.Commit();


            //^ try to launch Swal
            return RedirectToAction("details", new {id= AppointmentVm.DoctorId});

        }




        //======================
        // Set Appointment 
        //======================

        public async Task<IActionResult> GetAvailableTime(int id, int year, int month, int day)
        {
            List<DoctorAppointment> appointments = _doctorAppointmentRepository.GetAll(x => x.DoctorId == id && x.MeetingDate.Day == day && x.MeetingDate.Month == month && x.MeetingDate.Year == year).ToList();

            List<DateTime> TodaysMeetings = appointments.Select(x => x.MeetingDate).ToList();

            DateTime startDate = DateTime.UtcNow.Date.AddHours(8);

            if (year == DateTime.UtcNow.Year && month == DateTime.UtcNow.Month && day == DateTime.UtcNow.Day)
            {
                 startDate = DateTime.UtcNow.AddHours(4).AddMinutes(30 - DateTime.UtcNow.Minute % 30); ;

            }

            DateTime endDate = DateTime.UtcNow.Date.AddHours(20);


            List<DateTime> AvailableTimes = GetDateTimeIntervals(startDate, endDate)
            .Where(interval => !TodaysMeetings.Any(appointment =>
          appointment.TimeOfDay >= interval.TimeOfDay &&
          appointment.TimeOfDay < interval.AddMinutes(30).TimeOfDay))
            .ToList();

            List<string> result = AvailableTimes.Select(x => x.ToShortTimeString()).ToList();

            return Ok(result);
        }



        // Custom functions
        private static List<DateTime> GetDateTimeIntervals(DateTime startTime, DateTime endTime)
        {
            startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, (startTime.Minute / 30) * 30, 0);
            endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, (endTime.Minute / 30) * 30, 0);

            List<DateTime> result = new List<DateTime>();

            DateTime current = startTime;
            while (current <= endTime)
            {
                result.Add(current);
                current = current.AddMinutes(30);
            }

            return result;
        }



    }
}
