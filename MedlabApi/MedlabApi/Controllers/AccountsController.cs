using Medlab.Core.Entities;
using MedlabApi.Dtos.LoginDtos;
using MedlabApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MedlabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AccountsController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._jwtService = jwtService;
            this._configuration = configuration;
        }

        //===========================
        // Login
        //===========================

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return BadRequest();

            if (await _userManager.CheckPasswordAsync(user, "doctor123"))
                return BadRequest();

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);


            return Ok(new { token = _jwtService.GenerateJwtToken(user, roles, _configuration) });

        }

        //===========================
        //  IsAuthenticated
        //===========================

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("isAuthenticated")]
        public IActionResult isAuthenticated()
        {
            return Ok();
        }
        [Authorize]
        [HttpGet("getRoles")]

        //===========================
        //  Get roles / role of the current user
        //===========================
        public async Task<IActionResult> getRoles()
        {
            var user =await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            var roles =await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }


        //===========================
        // Get ALl users
        //===========================
        [Authorize(Roles ="SuperAdmin")]
        [HttpGet("roles/all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }


 








        //[HttpGet("CreateRoles")]
        //public async Task<IActionResult> Create()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    await _roleManager.CreateAsync(new IdentityRole("Visitor"));
        //    await _roleManager.CreateAsync(new IdentityRole("Doctor"));


        //    return Ok();
        //}


        //[HttpPost("createAdmin")]
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser
        //    {

        //        Fullname = "Samir Tahirli",
        //        UserName = "SamirAdmin",
        //        Email = "Samir@gmail.com",


        //    };
        //    await _userManager.CreateAsync(admin, "admin123");
        //    await _userManager.AddToRoleAsync(admin, "Admin");

        //    AppUser SuperAdmin = new AppUser
        //    {
        //        Fullname = "Tahir Tahirli",
        //        UserName = "SuperAdmin",
        //        Email = "Tahir@gmail.com",


        //    };
        //    await _userManager.CreateAsync(SuperAdmin, "admin123");
        //    await _userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");


        //    return Ok();
        //}




    }
}
