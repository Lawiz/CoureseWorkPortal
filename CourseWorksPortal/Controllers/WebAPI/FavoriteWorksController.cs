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
    public class FavoriteWorksController : Controller
    {
        AppDbContext db;

        public FavoriteWorksController(AppDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IEnumerable<FavoriteWork> Get() => db.FavoriteWorks.ToList();

        [HttpGet("{id}")]
        public IEnumerable<FavoriteWork> GetByUserId(int id)
        {
            return db.FavoriteWorks.Where(x => x.Username_Id == id).ToList();
        }

        [HttpPost]
        public IActionResult Add([FromBody]FavoriteWork favoriteWork)
        {
            if (favoriteWork == null) return BadRequest();

            db.FavoriteWorks.Add(favoriteWork);
            db.SaveChanges();
            return Ok(favoriteWork);
        }

        [HttpPut]
        public IActionResult Update([FromBody]FavoriteWork favoriteWork)
        {
            if (favoriteWork == null) return BadRequest();
            if (!db.FavoriteWorks.Any(x => x.Id == favoriteWork.Id)) return NotFound();

            db.Update(favoriteWork);
            db.SaveChanges();
            return Ok(favoriteWork);
        }
        
        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            FavoriteWork favoriteWork = db.FavoriteWorks.FirstOrDefault(x => x.CourseWork_Id == id);

            if (favoriteWork == null) return NotFound();

            db.FavoriteWorks.Remove(favoriteWork);
            db.SaveChanges();
            return Ok(favoriteWork);
        }
    }
}
