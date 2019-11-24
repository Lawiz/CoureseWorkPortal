using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWorksPortal.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseWorksPortal.Controllers.WebAPI
{
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        AppDbContext db;

        public UsersController(AppDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IEnumerable<User> Get() => db.Users.ToList();

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            return new ObjectResult(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody]User user)
        {
            if (user == null) return BadRequest();

            db.Users.Add(user);
            db.SaveChanges();
            return Ok(user);
        }
    }
}
