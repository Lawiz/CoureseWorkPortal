using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CourseWorksPortal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWorksPortal.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private AppDbContext db;

        public UserController(AppDbContext context)
        {
            db = context;
        }

        public async Task<IActionResult> AllUsers()
        {
            return View(await db.Users.ToListAsync());
        }

        // создание нового юзера
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            User dublicate_user = await db.Users.FirstOrDefaultAsync(p => p.Username == user.Username);
            if (dublicate_user == null)
            {
                //pass hashing
                user.Password = HashingHelper.getHash(user.Username, user.Password);

                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("AllUsers");
            }
            else return RedirectToAction("Create");
        }

        // редактирование существующего юзера
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                user.Password = "";
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            User old_user = await db.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == user.Id);
            if (user.Password != null) user.Password = HashingHelper.getHash(user.Username, user.Password);
            else if (user.Password == null) user.Password = old_user.Password;

            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("AllUsers");
        }

        // удаление существующего юзера
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("AllUsers");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("AllUsers");
                }
            }
            return NotFound();
        }
    }
}
