using Microsoft.AspNetCore.Http;
using System.IO;
using BCrypt.Net;

namespace ASM_APDP.Services
{
    public class AuthManagement
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string studentFilePath = "wwwroot/students.csv";
        private readonly string adminFilePath = "wwwroot/admins.csv";

        public AuthManagement(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            // Kiểm tra Admin
            if (CheckCredentials(adminFilePath, username, password))
            {
                SetSession(username, "Admin");
                return "Admin";
            }

            // Kiểm tra Student
            if (CheckCredentials(studentFilePath, username, password))
            {
                SetSession(username, "Student");
                return "Student";
            }

            return null;
        }

        private bool CheckCredentials(string filePath, string username, string password)
        {
            if (!File.Exists(filePath)) return false;
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var data = line.Split(",");
                if (data.Length >= 2 && data[0] == username && BCrypt.Net.BCrypt.Verify(password, data[1]))
                    return true;
            }
            return false;
        }

        private void SetSession(string username, string role)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Session.SetString("username", username);
                context.Session.SetString("role", role);
            }
        }

        public void Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Session.Clear();
            }
        }
    }
}
