using FluentValidation;
using Medlab.Core.Entities;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
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
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult>  Login(LoginViewModel LoginVM, string? ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
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
            if (!user.EmailConfirmed)
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action(nameof(ConfirmEmail), "account", new { token = token, email = user.Email }, Request.Scheme);

                ModelState.AddModelError("", $"Please Verify Your Account");
                ViewBag.VerificationLink = url;
                ViewBag.Email = user.Email;
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
                    Fullname = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    PhoneNumber = info.Principal.FindFirst(ClaimTypes.MobilePhone).Value

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
        [ValidateAntiForgeryToken]

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

            // Send Email Verification

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var url = Url.Action(nameof(ConfirmEmail), "account", new { token = token, email = newUser.Email }, Request.Scheme);

            SendMail(newUser.Email, "Email Verification", $"Click <a href=\"{url}\" >here</a> to verify your email");

            return RedirectToAction("index" , "home");
        }
        // Profile
        public IActionResult Profile()
        {
            return View();
        }
        public void SendMail( string To,string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("tahirtahirli2002@gmail.com", "jgauwtdjhnwgxaib");
            smtpClient.EnableSsl = true;

            // message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tahirtahirli2002@gmail.com");
            mailMessage.To.Add(To);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            
            smtpClient.Send(mailMessage);
        }

        public IActionResult ResendMail(string To, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("tahirtahirli2002@gmail.com", "jgauwtdjhnwgxaib");
            smtpClient.EnableSsl = true;

            // message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tahirtahirli2002@gmail.com");
            mailMessage.To.Add(To);
            mailMessage.Subject = subject;
            mailMessage.Body = message;


            smtpClient.Send(mailMessage);
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult>  ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

           var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return NotFound();
            await _userManager.RemoveFromRoleAsync(user, "Visitor");
            await _userManager.AddToRoleAsync(user, "Member");


            return RedirectToAction(nameof(Login));
        }
    }
}
