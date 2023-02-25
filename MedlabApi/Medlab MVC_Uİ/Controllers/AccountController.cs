using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Medlab_MVC_Uİ.Helpers;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Policy;


namespace Medlab_MVC_Uİ.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly MedlabDbContext _context; // for begin transaction
        private readonly IWebHostEnvironment _env;
        private readonly IDoctorAppointmentRepository _doctorAppointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public AccountController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            MedlabDbContext context,
            IMapper mapper,
            IWebHostEnvironment env,
            IDoctorAppointmentRepository doctorAppointmentRepository,
            IDoctorRepository doctorRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository
            )
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            _mapper = mapper;
            _context = context;
            _env = env;
            _doctorAppointmentRepository = doctorAppointmentRepository;
            _doctorRepository = doctorRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        //======================
        // Login
        //======================

        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel LoginVM, string? ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (!ModelState.IsValid)
                return View(LoginVM);

            var user = await _userManager.FindByNameAsync(LoginVM.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "UserName or password is incorrect");
                return View(LoginVM);
            }


            var userRoles = await _userManager.GetRolesAsync(user);

            if (!user.EmailConfirmed)
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action(nameof(ConfirmEmail), "account", new { token = token, email = user.Email }, Request.Scheme);

                ModelState.AddModelError("", $"Please Verify Your Account");
                ViewBag.VerificationLink = url;
                ViewBag.Email = user.Email;
                return View(LoginVM);
            }
            if (!userRoles.Contains("Member"))
            {
                ModelState.AddModelError("", "UserName or password is incorrect");
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
                ModelState.AddModelError("", "UserName or password is incorrect");
                return View(LoginVM);
            }

            return RedirectToAction("index", "home");
        }

        //======================
        // Google Login
        //======================
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
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


        //======================
        // facebook login
        //======================
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            string redirectUrl = Url.Action("FacebookResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }


        [AllowAnonymous]
        public async Task<IActionResult> FacebookResponse()
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

            return RedirectToAction("index", "home");
        }



        //======================
        // Register
        //======================
        public IActionResult Register()
        {
            return View();
            //======================
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterViewModel RegisterVm)
        {
            if (!ModelState.IsValid)
            {
                return View(RegisterVm);
            }

            if (await _userManager.FindByNameAsync(RegisterVm.Username) != null)
            {
                ModelState.AddModelError("UserName", "This UserName Is Already Taken");
                return View(RegisterVm);
            }
            if (await _userManager.FindByEmailAsync(RegisterVm.Email) != null)
            {
                ModelState.AddModelError("Emai", "This Email Has been Used");
                return View(RegisterVm);
            }

            //^ use mapper
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

            await _userManager.AddToRoleAsync(newUser, "Visitor");

            // Send Email Verification

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var url = Url.Action(nameof(ConfirmEmail), "account", new { token = token, email = newUser.Email }, Request.Scheme);

            SendMail(newUser.Email, "Email Verification", $"Click <a href=\"{url}\" >here</a> to verify your email");

            return RedirectToAction("login");
        }


        //======================
        // Forgot Password
        //======================
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel PasswordVm)
        {
            if (!ModelState.IsValid)
                return View(PasswordVm);


            var user = await _userManager.FindByEmailAsync(PasswordVm.Email);
            if (user == null)
                return NotFound();

            if(user.PasswordHash == null)
            {
                ModelState.AddModelError("Email", "this User Is logged in With Third Party Apps");
                return View();
            }

            if(user.LastRequestedEmailAt.AddMinutes(1) <= DateTime.UtcNow.AddHours(4))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action("VerifyPasswordReset", "account", new { email = user.Email, token = token }, Request.Scheme);

                SendMail(PasswordVm.Email, "Password Reset", $"Click <a href='{url}' >here</a> to verify your email");

                user.LastRequestedEmailAt = DateTime.UtcNow.AddHours(4);
                await _userManager.UpdateAsync(user);
            }

            return View();
        }
        //======================
        // Verify password Reset
        //======================

        public async Task<IActionResult> VerifyPasswordReset(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
                return NotFound();

            TempData["Token"] = token;
            TempData["Email"] = email;
            return RedirectToAction("resetPassword");
        }
        //======================
        // Reset Password
        //======================
        public IActionResult ResetPassword()
        {
            var email = TempData["Email"];
            var token = TempData["Token"];


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVeiwModel ResetVm)
        {
            if (ResetVm.Email == null || ResetVm.Token == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["Email"] = ResetVm.Email;
                TempData["Token"] = ResetVm.Token;
                return View();
            }

            var user = await _userManager.FindByEmailAsync(ResetVm.Email);

            if (user == null)
                return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, ResetVm.Token, ResetVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["Email"] = ResetVm.Email;
                TempData["Token"] = ResetVm.Token;
                return View();
            }
            return RedirectToAction("login");

        }

        //======================
        // Profile
        //======================
        [Authorize(Roles = "Member, Visitor")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            ProfileViewModel model = new ProfileViewModel();
            model.EditProfileViewModel = _mapper.Map<EditProfileViewModel>(user);
            model.DoctorAppointments = _doctorAppointmentRepository.GetAppointmentsIncludingUsers(x => x.AppUserId == user.Id).OrderByDescending(x => x.MeetingDate).Take(20).ToList();
            model.Orders = _orderRepository.GetOrdersWithProducts(user.Id);
            model.UserPhoto = $"Users/{user.ImageUrl}";
            model.Fullname = user.Fullname;
            // Getting user Orders


            if (user.PasswordHash == null)
                model.EditProfileViewModel.IsExternalUser = true;

            //If user Is Doctor then, Get doctor Blogs
            if (User.IsInRole("Doctor"))
            {
                int doctorId = user.DoctorId ?? 0;
                var doctor = _doctorRepository.GetDoctor(doctorId);
                if (doctor == null)
                    return NotFound();

                model.Doctor = doctor;
                model.UserPhoto = $"DoCtors/{doctor.ImageUrl}";
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(EditProfileViewModel ProfileVm)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();



            ProfileViewModel model = new ProfileViewModel();
            model.EditProfileViewModel = ProfileVm;
            model.EditProfileViewModel.ImageUrl = user.ImageUrl;
            model.DoctorAppointments = _doctorAppointmentRepository.GetAppointmentsIncludingUsers(x => x.AppUserId == user.Id).OrderByDescending(x => x.MeetingDate).Take(20).ToList(); ;
            model.Orders = _orderRepository.GetOrdersWithProducts(user.Id);

            if (user.PasswordHash == null)
                model.EditProfileViewModel.IsExternalUser = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var isEmailChanged = false;
            using (var transaction = _context.Database.BeginTransaction())
            {

                IdentityResult isPasswordUpdated = IdentityResult.Success;
                IdentityResult isUserUpdated = IdentityResult.Success;

                if (ProfileVm.Email.ToLower() != user.Email.ToLower())
                {

                    if (await _userManager.FindByEmailAsync(ProfileVm.Email) == null)
                    {

                        // Send Email Verification
                        user.Email = ProfileVm.Email;
                        user.EmailConfirmed = false;
                        isEmailChanged = true;
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var url = Url.Action(nameof(ConfirmEmail), "account", new { token = token, email = user.Email }, Request.Scheme);

                        SendMail(user.Email, "Email Verification", $"Click <a href=\"{url}\" >here</a> to verify your email");

                    }
                    else
                    {
                        ModelState.AddModelError("Email", "This Email Has Been Used");
                    }

                }

                if (ProfileVm.UserName.ToLower() != user.UserName.ToLower())
                {
                    if (await _userManager.FindByNameAsync(ProfileVm.UserName) == null)
                        user.UserName = ProfileVm.UserName;
                    else
                    {
                        ModelState.AddModelError("UserName", "This Username Is Already Taken");

                    }
                }

                if (ProfileVm.Password != null)
                {
                    if (ProfileVm.NewPassword == null || ProfileVm.ConfirmPassword == null)
                    {
                        ModelState.AddModelError("Password", "Fill To Change Password");
                        ModelState.AddModelError("NewPassword", "Fill To Change Password");
                        ModelState.AddModelError("ConfirmPassword", "Fill To Change Password");
                        return View(model);
                    }
                    isPasswordUpdated = await _userManager.ChangePasswordAsync(user, ProfileVm.Password, ProfileVm.NewPassword);
                    if (!isPasswordUpdated.Succeeded)
                    {

                        foreach (var error in isPasswordUpdated.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                if (ProfileVm.ImageFile != null)
                {
                    if (user.ImageUrl != "DEFAULT-USER.jpg")
                        FileManager.Delete(_env.WebRootPath, "Assets/Uploads/Users", user.ImageUrl);

                    user.ImageUrl = FileManager.Save(ProfileVm.ImageFile, _env.WebRootPath, "Assets/Uploads/Users", 200);
                }



                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                user.Fullname = ProfileVm.Fullname;
                user.PhoneNumber = ProfileVm.PhoneNumber;

                isUserUpdated = await _userManager.UpdateAsync(user);



                if (isUserUpdated.Succeeded && isPasswordUpdated.Succeeded)
                {
                    _context.SaveChanges();
                    transaction.Commit();
                }
                else
                {
                    ModelState.AddModelError("", "something went wrong");
                    return View(model);
                }

            }








            if (isEmailChanged)
                await _signInManager.SignOutAsync();


            return RedirectToAction("profile");
        }

        //======================
        // Log Out
        //======================

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }


        //======================
        // Functions
        //======================
        public void SendMail(string To, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("tahirtahirli2002@gmail.com", "lsieytvhoimyzhbi");
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
            smtpClient.Credentials = new System.Net.NetworkCredential("tahirtahirli2002@gmail.com", "lsieytvhoimyzhbi");
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

        public async Task<IActionResult> ConfirmEmail(string token, string email)
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
