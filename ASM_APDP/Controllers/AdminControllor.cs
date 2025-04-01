using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using System.IO;

namespace ASM_APDP.Controllers
{
    public class AdminController : Controller
    {
        private readonly string teacherFile = "wwwroot/data/Teacher.csv";

        public IActionResult AdminHome()
        {
            return View();
        }

        public IActionResult CreateTeacher()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTeacher(TeacherModel model)
        {
            if (!System.IO.File.Exists(teacherFile))
            {
                System.IO.File.WriteAllText(teacherFile, "");
            }

            System.IO.File.AppendAllText(teacherFile, $"{model.Email},{model.Password},Teacher\n");
            return RedirectToAction("AdminHome");
        }
    }
}
