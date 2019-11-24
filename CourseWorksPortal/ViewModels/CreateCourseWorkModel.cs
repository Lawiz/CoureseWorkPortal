using System;
using Microsoft.AspNetCore.Http;

namespace CourseWorksPortal.ViewModels
{
    public class CreateCourseWorkModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
