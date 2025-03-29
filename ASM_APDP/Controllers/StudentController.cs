using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using BCrypt.Net;

namespace ASM_APDP.Controllers
{
    public class StudentController : Controller
    {
        private readonly string filePath = "wwwroot/students.csv";

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
            if (HttpContext.Session.GetString("username") == null)
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
