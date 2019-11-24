using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorksPortal.Models;
using CourseWorksPortal.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWorksPortal.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CourseWorkController : Controller
    {
        private AppDbContext db;

        public CourseWorkController(AppDbContext context)
        {
            db = context;
        }

        // показать все работы
        public async Task<IActionResult> AllCourseWorks()
        {
            return View(await db.CourseWorks.ToListAsync());
        }

        // детализация информации о курсовом
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                CourseWork courseWork = await db.CourseWorks.FirstOrDefaultAsync(p => p.Id == id);
                if (courseWork != null)
                    return View(courseWork);
            }
            return NotFound();
        }

        // создание новой курсовой работы
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseWorkModel model) 
        {
            if (ModelState.IsValid)
            {
                var courseWork = new CourseWork()
                {
                    Name = model.Name,
                    Description = model.Description
                };

                using (var memoryStream = new MemoryStream())
                {
                    if (model.File != null)
                    {
                        courseWork.File_name = model.File.FileName;
                        await model.File.CopyToAsync(memoryStream);
                        courseWork.File = Convert.ToBase64String(memoryStream.ToArray());
                    }
                    else
                    {
                        return RedirectToAction("Create");
                    }
                }

                db.CourseWorks.Add(courseWork);
                await db.SaveChangesAsync();
                return RedirectToAction("AllCourseWorks");
            }

            return null;
        }

        // редактирование существующей курсовой работы
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                CourseWork courseWork = await db.CourseWorks.FirstOrDefaultAsync(p => p.Id == id);

                if (courseWork != null)
                {
                    CreateCourseWorkModel courseWorkModel = new CreateCourseWorkModel()
                    {
                        Id = courseWork.Id,
                        Name = courseWork.Name,
                        Description = courseWork.Description
                    };

                    return View(courseWorkModel);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateCourseWorkModel model)
        {
            if (ModelState.IsValid)
            {
                CourseWork old_courseWork = await db.CourseWorks.FirstOrDefaultAsync(p => p.Id == model.Id);

                old_courseWork.Name = model.Name;
                old_courseWork.Description = model.Description;

                using (var memoryStream = new MemoryStream())
                {
                    if (model.File != null)
                    {
                        old_courseWork.File_name = model.File.FileName;
                        await model.File.CopyToAsync(memoryStream);
                        old_courseWork.File = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                db.CourseWorks.Update(old_courseWork);
                await db.SaveChangesAsync();
                return RedirectToAction("AllCourseWorks");
            }

            return null;
        }

        // удаление существующей курсовой работы
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                CourseWork courseWork = await db.CourseWorks.FirstOrDefaultAsync(p => p.Id == id);
                if (courseWork != null)
                    return View(courseWork);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                CourseWork courseWork = await db.CourseWorks.FirstOrDefaultAsync(p => p.Id == id);
                if (courseWork != null)
                {
                    db.CourseWorks.Remove(courseWork);
                    await db.SaveChangesAsync();
                    return RedirectToAction("AllCourseWorks");
                }
            }
            return NotFound();
        }
    }
}
