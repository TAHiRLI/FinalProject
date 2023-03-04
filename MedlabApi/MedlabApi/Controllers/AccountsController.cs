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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return BadRequest();

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return NotFound();
            var roles = await _userManager.GetRolesAsync(user);


            return Ok(new { token = _jwtService.GenerateJwtToken(user, roles, _configuration) });

        }


        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("isAuthenticated")]
        public IActionResult isAuthenticated()
        {
            return Ok();
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
