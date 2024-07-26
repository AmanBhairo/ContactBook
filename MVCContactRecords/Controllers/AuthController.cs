using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCContactRecords.Data;
using MVCContactRecords.Models;
using MVCContactRecords.Services.Contract;
using MVCContactRecords.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCContactRecords.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                //Password Strength
                var message = _authService.RegisterUserService(register);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    TempData["ErrorMessage"] = message;
                    return View(register);
                }
                //User Exist
                return RedirectToAction("RegisterSuccess");
            }
            return View(register);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var message = _authService.LoginUserService(login);
                if (message == "Invalid username or password")
                {
                    TempData["ErrorMessage"] = "Invalid username or password";
                    return View(login);
                }
                else if (message == "Something went wrong,please try after sometime.")
                {
                    TempData["ErrorMessage"] = "Invalid username or password";
                    return View(login);
                }
                else
                {
                    string token = message;
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                    return RedirectToAction("Index1", "Contact");
                }

            }
            return View(login);
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
