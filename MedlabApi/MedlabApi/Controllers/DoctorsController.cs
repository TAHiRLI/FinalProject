using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.DoctorDtos;
using MedlabApi.Dtos.ProductDtos;
using MedlabApi.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop.Infrastructure;
using System;
using System.Net.Mail;
using System.Text;

namespace MedlabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public DoctorsController(UserManager<AppUser> userManager, IMapper mapper, IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository, IHostEnvironment env, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _env = env;
            _configuration = configuration;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var doctors = _doctorRepository.GetAll(x => true, "Department", "AppUser", "Blogs").OrderByDescending(x => x.CreatedAt).ToList();
            var dto = _mapper.Map<List<DoctorListItemDto>>(doctors);

            return Ok(dto);
        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorRepository.GetAsync(x => x.Id == id, "Department", "AppUser", "Blogs");
            var dto = _mapper.Map<DoctorGetDto>(doctor);
            return Ok(dto);
        }

        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] DoctorPostDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return BadRequest(new { errors = new { Email = new[] { "This Email Has Already Been Used !!!" } } });


            if (await _departmentRepository.GetAsync(x => x.Id == dto.DepartmentId) == null)
                return BadRequest(new { errors = new { DepartmentId = new[] { "Department Does Not Exist !!!" } } });


            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            string imageName = FileManager.Save(dto.Image, imagePath, "Uploads/Doctors", 200);

            Doctor newDoctor = new Doctor
            {
                Fullname = dto.Fullname,
                Email = dto.Email,
                Facebook = dto.Facebook,
                Twitter = dto.Twitter,
                Instagram = dto.Instagram,
                IsFeatured = dto.IsFeatured,
                Gender = dto.Gender,
                DepartmentId = dto.DepartmentId,
                Desc = dto.Desc,
                DetailedDesc = dto.DetailedDesc,
                ImageUrl = imageName,
                Salary = dto.Salary,
                MeetingPrice = dto.MeetingPrice,
                Office = dto.Office,
                Positon = dto.Positon,

            };

            AppUser doctorUser = new AppUser
            {
                UserName = dto.Email,
                Fullname = dto.Fullname,
                Email = dto.Email,
                ImageUrl = imageName,
                Doctor = newDoctor,
            };



            var userCreate = await _userManager.CreateAsync(doctorUser, "doctor123");
            await _userManager.AddToRoleAsync(doctorUser, "Doctor");
            await _userManager.AddToRoleAsync(doctorUser, "Member");
            doctorUser.EmailConfirmed = true;

            SendMail(doctorUser.Email, "Set Your Medlab Password", $"You have been registered as doctor in Medlab.com. Your Username is {doctorUser.Email}. For the firs login you will be redirected to Set Password page");

            await _userManager.UpdateAsync(doctorUser);
            await _doctorRepository.CommitAsync();

            var result = _mapper.Map<DoctorGetDto>(newDoctor);

            return Ok(result);
        }

        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] DoctorPutDto dto)
        {
            var doctor = await _doctorRepository.GetAsync(x => x.Id == id, "AppUser");
            if (doctor == null)
                return NotFound();

            if (doctor.Email?.ToLower() != dto.Email)
            {
                if (await _userManager.FindByEmailAsync(dto.Email) != null)
                    return BadRequest(new { errors = new { Email = new[] { "This Email Has Already Been Used !!!" } } });
             

                SendMail(dto.Email, "Set Your Medlab Password", $"You have been registered as doctor in Medlab.com. Your Username is {dto.Email}. For the firs login you will be redirected to Set Password page");


                doctor.Email = dto.Email;
                doctor.AppUser.Email = dto.Email;
                doctor.AppUser.UserName = dto.Email;

            }
            if (!_departmentRepository.Any(x => x.Id == dto.DepartmentId))
                return BadRequest(new { errors = new { DepartmentId = new[] { "Department Does Not Exist !!!" } } });
            if (dto.Image != null)
            {
                // delete image

                var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
                var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

                FileManager.Delete(imagePath, "Uploads/Doctors", doctor.ImageUrl);
                doctor.ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/Doctors", 200);
                doctor.AppUser.ImageUrl = doctor.ImageUrl;
            }




            doctor.DepartmentId = dto.DepartmentId;
            doctor.Fullname = dto.Fullname;
            doctor.AppUser.Fullname = dto.Fullname;
            doctor.Positon = dto.Positon;
            doctor.Office = dto.Office;
            doctor.Salary = dto.Salary;
            doctor.MeetingPrice = dto.MeetingPrice;
            doctor.IsFeatured = dto.IsFeatured;
            doctor.Desc = dto.Desc;
            doctor.DetailedDesc = dto.Desc;
            doctor.Instagram = dto.Instagram;
            doctor.Facebook = dto.Facebook;
            doctor.Twitter = dto.Twitter;


            await _doctorRepository.CommitAsync();
            await _userManager.UpdateAsync(doctor.AppUser);

            var result = _mapper.Map<DoctorGetDto>(doctor);

            return Ok(result);
        }
        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorRepository.GetAsync(x => x.Id == id);
            if (doctor == null)
                return NotFound();

            var user = await _userManager.FindByEmailAsync(doctor.Email);
            if (user == null)
                return NotFound();

            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");


            // delete doctor image 
            FileManager.Delete(imagePath, "Uploads/Doctors", doctor.ImageUrl);

            await _userManager.DeleteAsync(user);
            _doctorRepository.Delete(doctor);

            _doctorRepository.Commit();


            return NoContent();
        }
        //=========================
        // Functions
        //=========================
        private void SendMail(string To, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("tahirtahirli2002@gmail.com", _configuration.GetSection("GoogleAuth:AppPassword").Value);
            smtpClient.EnableSsl = true;

            // message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tahirtahirli2002@gmail.com");
            mailMessage.To.Add(To);
            mailMessage.Subject = subject;
            mailMessage.Body = message;


            smtpClient.Send(mailMessage);
        }
    }
}
