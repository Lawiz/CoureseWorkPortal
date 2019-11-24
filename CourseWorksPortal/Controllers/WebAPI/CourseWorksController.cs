using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWorksPortal.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorksPortal.Controllers.WebAPI
{
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CourseWorksController : Controller
    {
        AppDbContext db;

        public CourseWorksController(AppDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IEnumerable<dynamic> Get() 
            => db.CourseWorks.Select(p => new { p.Id, p.Name, p.Description, p.File_name }).ToList();

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            CourseWork courseWork = db.CourseWorks.FirstOrDefault(x => x.Id == id);
            if (courseWork != null)
                return new ObjectResult(courseWork);
            return NotFound();
        }

        [HttpGet("{id}")]
        public dynamic GetFile(int id)
        {
            return db.CourseWorks.Where(p => p.Id == id).Select(p => new { p.File, p.File_name }).FirstOrDefault();

        }
    }
}
