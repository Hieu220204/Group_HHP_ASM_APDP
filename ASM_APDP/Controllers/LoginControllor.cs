using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using System.IO;
using System.Linq;

namespace ASM_APDP.Controllers
{
    public class LoginController : Controller
    {
        private readonly string studentFile = "wwwroot/data/Student.csv";
        private readonly string teacherFile = "wwwroot/data/Teacher.csv";
        private readonly string adminEmail = "admin";
        private readonly string adminPassword = "admin123";

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (model.Email == adminEmail && model.Password == adminPassword)
            {
                return RedirectToAction("AdminHome", "Admin");
            }

            if (System.IO.File.Exists(studentFile))
            {
                var students = System.IO.File.ReadAllLines(studentFile)
                    .Select(line => line.Split(','))
                    .Where(data => data.Length == 3 && data[1] == model.Email && data[2] == model.Password)
                    .ToList();

                if (students.Any())
                {
                    return RedirectToAction("StudentHome", "Student");
                }
            }

            if (System.IO.File.Exists(teacherFile))
            {
                var teachers = System.IO.File.ReadAllLines(teacherFile)
                    .Select(line => line.Split(','))
                    .Where(data => data.Length == 3 && data[0] == model.Email && data[1] == model.Password)
                    .ToList();

                if (teachers.Any())
                {
                    return RedirectToAction("TeacherHome", "Teacher");
                }
            }

            ViewBag.Message = "Invalid login credentials";
            return View();
        }
    }
}
