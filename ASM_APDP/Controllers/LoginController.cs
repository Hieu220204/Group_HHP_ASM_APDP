using Microsoft.AspNetCore.Mvc;
using System.IO;
using BCrypt.Net;

namespace ASM_APDP.Controllers
{
    public class LoginController : Controller
    {
        private readonly string studentFilePath = "wwwroot/students.csv";
        private readonly string adminFilePath = "wwwroot/admins.csv";

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
                // Kiểm tra tài khoản admin
                var admins = System.IO.File.ReadAllLines(adminFilePath);
                foreach (var line in admins)
                {
                    var data = line.Split(",");
                    if (data.Length >= 2 && data[0] == username && BCrypt.Net.BCrypt.Verify(password, data[1]))
                    {
                        HttpContext.Session.SetString("username", username);
                        HttpContext.Session.SetString("role", "admin");
                        return RedirectToAction("AdminHome", "Admin");
                    }
                }

                // Kiểm tra tài khoản student
                var students = System.IO.File.ReadAllLines(studentFilePath);
                foreach (var line in students)
                {
                    var data = line.Split(",");
                    if (data.Length >= 2 && data[0] == username && BCrypt.Net.BCrypt.Verify(password, data[1]))
                    {
                        HttpContext.Session.SetString("username", username);
                        HttpContext.Session.SetString("role", "student");
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
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
