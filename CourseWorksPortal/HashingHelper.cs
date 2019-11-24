using System;
using System.Security.Cryptography;
using System.Text;
using CourseWorksPortal.Models;

namespace CourseWorksPortal
{
    public class HashingHelper
    {
        public static string getHash(string username, string password)
        {
            string toHash = username + password;
            byte[] hash = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(toHash));
            string hashString = string.Empty;
            foreach (byte x in hash)
                hashString += string.Format("{0:x2}", x);
            return hashString;
        }
    }
}
