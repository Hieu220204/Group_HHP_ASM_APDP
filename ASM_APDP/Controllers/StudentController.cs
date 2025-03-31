using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using BCrypt.Net;

namespace ASM_APDP.Controllers
{
    public class StudentController : Controller
    {
        private readonly string filePath = "wwwroot/students.csv";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public IActionResult SaveStudent(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Message = "Username and Password cannot be empty!";
                return View("Register");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            string data = $"{username},{hashedPassword}\n";

            try
            {
                System.IO.File.AppendAllText(filePath, data, Encoding.UTF8);
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                ViewBag.Message = "Error saving student data.";
                return View("Register");
            }
        }

        public IActionResult StudentHome()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null || string.IsNullOrEmpty(session.GetString("username")))
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
