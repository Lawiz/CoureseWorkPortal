using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CourseWorksPortal
{
    public class AuthOptions
    {
        public const string ISSUER = "CourseWorksPortalServer"; // издатель токена
        public const string AUDIENCE = "http://localhost:51884/"; // потребитель токена
        const string KEY = "EaUNrgn8RD8NJwJbC3I5I82gqgP186";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 день

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
