using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CourseWorksPortal.Models;
using CourseWorksPortal.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWorksPortal.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        #region props
        private AppDbContext _dbContext;
        private IUserService _userService;
        #endregion

        #region ctor
        public UserController(AppDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }
        #endregion

        #region actions

        public async Task<IActionResult> AllUsers()
        {
            return View(await _userService.GetAll());
        }

        // создание нового юзера
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var res = await _userService.CreateUser(user);
            if (res.Result)
                return RedirectToAction("AllUsers");
            return RedirectToAction("Create");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _userService.GetUserForEdit(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            ///todo: informationEception
            var res = _userService.EditUser(user);
            return RedirectToAction("AllUsers");
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if((await _userService.ConfiemDelete(id)).Result)
                return RedirectToAction("AllUsers");
            return NotFound();
        }

        [HttpPost]
        public async void Delete(int? id)
        {
            await ConfirmDelete(id);
        }

        #endregion
        
    }
}
