using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Common;
using OnixLabs.Core.Text;
using System.Text;
using WebApp_Tak4.Models;
using WebApp_Tak4.Services.Auth;
using WebApp_Task4.Repositories;
using WebApp_Task4.Services;
using WebApp_Task4.ViewModels;

namespace WebApp_Task4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly EncryptionService _encryptionService;
        private readonly EmailService _emailService;
        private IMemoryCache _memoryCache;

        public AccountController(UserRepository userRepository, EncryptionService encryptionService, EmailService emailService, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _emailService = emailService;
            _memoryCache = memoryCache;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await _userRepository.GetByEmail(model.Email);
                if(user == null)
                {
                    ViewData["ErrorMessage"] = "Such user doesnt exist";
                    return View(model);
                }
                string inputPasswordHash = HashPassword(model.Password+model.Email);
                if(user.Passwordhash != inputPasswordHash)
                {
                    ViewData["ErrorMessage"] = "Wrong Password";
                    return View(model);
                }
                user.Lastlogin = DateTime.Now;
                await _userRepository.Update(user);

                var token = JwtAuthenticationService.GenerateJSONWebToken(user);

                if(model.RememberMe){

                    HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(14),
                        HttpOnly = true,
                        Secure = true
                    });
                }
                else
                {
                    HttpContext.Response.Cookies.Append("jwt", token);
                }


                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = CastUser(model);
                await _userRepository.Create(user);
                var token = JwtAuthenticationService.GenerateJSONWebToken(user);
                HttpContext.Response.Cookies.Append("jwt", token);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirmation(string email, string? recoveryCode)
        {
            User? user = await _userRepository.GetByEmail(email);                  
            if (!string.IsNullOrEmpty(recoveryCode) && recoveryCode == _memoryCache.Get<string>("recoveryCode"))
            {
                _memoryCache.Remove("recoveryCode");
                return View("PasswordRecovery");
            }
            if (user != null)
            {
                _memoryCache.Set("recoveryCode", await _emailService.SendRecoveryCode(user.Email));
                TempData["UserEmail"] = user.Email;
                return View(user);
            }
            ViewData["ErrorMessage"] = "User doesnt exist";
            return View("Login");
        }
        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordRecovery(string newPassword,string userEmail)
        {
            User? user = await _userRepository.GetByEmail(userEmail);
            var newHashPassword = _encryptionService.HashPassword(newPassword+userEmail).ToBase16().ToString();
            user.Passwordhash = newHashPassword;
            await _userRepository.Update(user);
            return View("Login");
        }

        private string HashPassword(string password)
        {
            return _encryptionService.HashPassword(password).ToBase16().ToString();
        }

        private User CastUser(RegisterViewModel model)
        {
            string passwordHash = HashPassword(model.Password + model.Email);
            return new User { Name = model.Name, Email = model.Email, Surname = model.Surname, Passwordhash = passwordHash};
        }

    }
}
