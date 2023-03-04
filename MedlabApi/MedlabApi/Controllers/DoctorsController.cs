using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.DepartmentDtos;
using MedlabApi.Dtos.DoctorDtos;
using MedlabApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

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

        public DoctorsController(UserManager<AppUser> userManager,IMapper mapper, IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository,IHostEnvironment env)
        {
            _mapper = mapper;
            _userManager = userManager;
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _env = env;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var doctors =  _doctorRepository.GetAll(x => true, "Department", "AppUser", "Blogs").OrderByDescending(x=> x.CreatedAt).ToList();
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
        public async Task<IActionResult> Create([FromForm]DoctorPostDto dto)
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

           var userCreate =  await _userManager.CreateAsync(doctorUser);
            await _doctorRepository.CommitAsync();

            var result = _mapper.Map<DoctorGetDto>(newDoctor); 

            return Ok(result);
        }
    }
}
