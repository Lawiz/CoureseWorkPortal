using System;
using System.ComponentModel.DataAnnotations;

namespace CourseWorksPortal.ViewModels
{
    public class LoginModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
