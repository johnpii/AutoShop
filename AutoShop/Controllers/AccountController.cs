using AutoShop.Interfaces;
using AutoShop.Models;
using AutoShop.Utilities;
using AutoShop.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepo;

        public AccountController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public IActionResult Regist()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Regist(RegistModel data)
        {
            if (ModelState.IsValid)
            {
                if (!_userRepo.ExistByEmail(data.Email))
                {
                    User user = new User();
                    user.Email = data.Email;
                    user.Username = data.Username;
                    user.Password = PasswordEncryption.EncryptPassword(data.Password);
                    user.Role = "user";
                    _userRepo.AddUser(user);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.Error = "Пользователь с такой почтой уже существует ! ";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Некорректные данные ! ";
                return View();
            }
        }

        public IActionResult Login(string? returnUrl)
        {
            if (returnUrl != null)
            {
                var options = new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(5),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("returnUrl", returnUrl, options);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel data)
        {
            if (ModelState.IsValid)
            {
                User? user = _userRepo.FindByEmailAndPassWord(data.Email, data.Password);
                if (user is null)
                {
                    ViewBag.Error = "Пользователь не найден ! ";
                    return View();
                }
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var options = new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(5),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("userId", user.Id.ToString(), options);
                await HttpContext.SignInAsync(claimsPrincipal);
                var returnUrl = Request.Cookies["returnUrl"];
                Response.Cookies.Delete("returnUrl");
                return Redirect(returnUrl ?? "/");
            }
            else
            {
                ViewBag.Error = "Некорректные данные ! ";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
