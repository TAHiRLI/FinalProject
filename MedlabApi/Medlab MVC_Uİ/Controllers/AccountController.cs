using FluentValidation;
using Medlab.Core.Entities;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Medlab_MVC_Uİ.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        public IActionResult Login(string? ReturnUrl = null)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Login(LoginViewModel LoginVM )
        {

            if (!ModelState.IsValid)
                return View(LoginVM);

            var user = await _userManager.FindByNameAsync(LoginVM.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(LoginVM);
            }


            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains("Member"))
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(LoginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, LoginVM.Password, LoginVM.RememberMe, true);


            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Too Many Attempts, Please Try Again");
                return View(LoginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(LoginVM);
            }

            return RedirectToAction("index", "home");
        }

        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Home");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index), "home");

            else
            {
                AppUser user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    EmailConfirmed = true,
                    Fullname = info.Principal.FindFirst(ClaimTypes.Email).Value

                };
                IdentityResult identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Member");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction(nameof(Index), "home");
                    }
                }
            }




            return NotFound();
        }



        // Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Register(RegisterViewModel RegisterVm)
        {
            if (!ModelState.IsValid)
            {
                return View(RegisterVm);
            }

            if(await _userManager.FindByNameAsync(RegisterVm.Username) != null)
            {
                ModelState.AddModelError("Username", "This Username Is Already Taken");
                return View(RegisterVm);
            }
            if (await _userManager.FindByEmailAsync(RegisterVm.Email) != null)
            {
                ModelState.AddModelError("Emai", "This Email Has been Used");
                return View(RegisterVm);
            }


            AppUser newUser = new AppUser
            {
                UserName = RegisterVm.Username,
                Fullname = RegisterVm.Fullname, 
                PhoneNumber = RegisterVm.PhoneNumber,
                PhoneNumberConfirmed = false,
                Email = RegisterVm.Email,
                EmailConfirmed = false,
            };

            var result = await _userManager.CreateAsync(newUser, RegisterVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(RegisterVm);
            }

            await _userManager.AddToRoleAsync(newUser,"Visitor");

            return Ok(RegisterVm);
        }
        // Profile
        public IActionResult Profile()
        {
            return View();
        }

    }
}
