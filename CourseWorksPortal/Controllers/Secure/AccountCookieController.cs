using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseWorksPortal.Models;
using CourseWorksPortal.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWorksPortal.Controllers
{
    public class AccountCookieController : Controller
    {
        private AppDbContext _dbContext;
        private String admin_role = "admin";

        public AccountCookieController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private  void CreateUser(User user)
        {
            User dublicate_user = _dbContext.Users.FirstOrDefault(p => p.Username == user.Username);
            if (dublicate_user == null)
            {
                //pass hashing
                user.Password = HashingHelper.getHash(user.Username, user.Password);

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
        }
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", "Заполенены не все поля");
                    return View(model);
                }

                model.Password = HashingHelper.getHash(model.Username, model.Password);
                User user = _dbContext.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Username);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Некорректные логин и (или) пароль");
            }
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "AccountCookie");
        }
        
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
