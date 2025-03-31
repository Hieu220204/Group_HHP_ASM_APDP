using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using BCrypt.Net;

namespace ASM_APDP.Controllers
{
    public class LoginController : Controller
    {
        private readonly string studentFilePath = "wwwroot/students.csv";
        private readonly string adminFilePath = "wwwroot/admins.csv";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public IActionResult Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Message = "Invalid credentials!";
                return View("Index");
            }

            try
            {
                // Kiểm tra admin
                var admins = System.IO.File.ReadAllLines(adminFilePath);
                foreach (var line in admins)
                {
                    var data = line.Split(",");
                    if (data.Length >= 2 && data[0] == username && BCrypt.Net.BCrypt.Verify(password, data[1]))
                    {
                        var session = _httpContextAccessor.HttpContext?.Session;
                        if (session != null)
                        {
                            session.SetString("username", username);
                            session.SetString("role", "admin");
                        }
                        return RedirectToAction("AdminHome", "Admin");
                    }
                }

                // Kiểm tra student
                var students = System.IO.File.ReadAllLines(studentFilePath);
                foreach (var line in students)
                {
                    var data = line.Split(",");
                    if (data.Length >= 2 && data[0] == username && BCrypt.Net.BCrypt.Verify(password, data[1]))
                    {
                        var session = _httpContextAccessor.HttpContext?.Session;
                        if (session != null)
                        {
                            session.SetString("username", username);
                            session.SetString("role", "student");
                        }
                        return RedirectToAction("StudentHome", "Student");
                    }
                }
            }
            catch
            {
                ViewBag.Message = "Error reading accounts.";
            }

            ViewBag.Message = "Invalid credentials!";
            return View("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
