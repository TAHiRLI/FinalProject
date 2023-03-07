using AutoMapper;
using Medlab.Core.Entities;
using MedlabApi.Dtos.UserDtos;
using MedlabApi.Helpers;
using MedlabApi.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedlabApi.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public UsersController(IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
            _configuration = configuration;
        }
        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var users = _userManager.Users.Where(x => x.IsAdmin != true).ToList();

            var dto = _mapper.Map<List<UserGetDto>>(users);

            return Ok(dto);
        }
        //=========================
        // Get All admins
        //=========================
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("Admins/All")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var users = _userManager.Users.Where(x => x.IsAdmin == true).ToList();

            var dto = _mapper.Map<List<UserGetDto>>(users);

            return Ok(dto);
        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            var dto = _mapper.Map<UserGetDto>(user);
            dto.Role = roles[0];
            return Ok(dto);
        }

        //===========================
        // Create admin 
        //===========================
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Admin")]
        public async Task<IActionResult> CreateAdmin([FromForm] UserPostDto dto)
        {
            var role = await _roleManager.FindByNameAsync(dto.Role);
            if (role == null)
                return BadRequest(new { errors = new { Role = new[] { "Role Does Not Exist !!!" } } });

            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return BadRequest(new { errors = new { Email = new[] { "This Email Has Already Been Used !!!" } } });
            if (await _userManager.FindByNameAsync(dto.Username) != null)
                return BadRequest(new { errors = new { Username = new[] { "Username Has Already Been Used !!!" } } });


            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");


            AppUser user = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Fullname = dto.Fullname,
                IsAdmin = true,
                ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/Users", 200),

            };

            await _userManager.CreateAsync(user, "doctor123");
            await _userManager.AddToRoleAsync(user, role.Name);
            await _userManager.UpdateAsync(user);

            EmailService emailService = new EmailService(_configuration);

            emailService.SendMail(user.Email, "Please Set Your Password", $"You Have been registered in medlab.com. Your username is {user.UserName} to set your password click this link \n Link:  {_configuration.GetSection("Mvc:Path").Value}Account/ForgotPassword");

            var result = _mapper.Map<UserGetDto>(user);

            return Ok(result);

        }

        //===========================
        // Edit admin 
        //===========================
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("Admin/{id}")]
        public async Task<IActionResult> EditAdmin(string id, [FromForm] UserPutDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var role = await _roleManager.FindByNameAsync(dto.Role);
            if (role == null)
                return BadRequest(new { errors = new { Role = new[] { "Role Does Not Exist !!!" } } });

            var testUser = await _userManager.FindByEmailAsync(dto.Email);
            if (testUser != null && testUser.Id != id)
                return BadRequest(new { errors = new { Email = new[] { "This Email Has Already Been Used !!!" } } });

            testUser = await _userManager.FindByNameAsync(dto.Username);

            if (testUser != null && testUser.Id != id)
                return BadRequest(new { errors = new { Username = new[] { "Username Has Already Been Used !!!" } } });

            if (dto.Email?.ToLower() != user.Email?.ToLower())
            {
                user.Email = dto.Email;
            }


            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            if (dto.Image != null)
            {
                if (user.ImageUrl != "DEFAULT-USER.jpg")
                    FileManager.Delete(imagePath, "Uploads/Users", user.ImageUrl);

                user.ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/Users", 200);
            }

            user.Fullname = dto.Fullname;
            user.PhoneNumber = dto.PhoneNumber;

            // clear all the roles and add the posting value
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRoleAsync(user, role.Name);
            await _userManager.UpdateAsync(user);

            var result = _mapper.Map<UserGetDto>(user);

            return Ok(result);
        }
    }
}
