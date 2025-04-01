using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using System.IO;

namespace ASM_APDP.Controllers
{
    public class RegisterController : Controller
    {
        private readonly string studentFile = "wwwroot/data/Student.csv";

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!System.IO.File.Exists(studentFile))
            {
                System.IO.File.WriteAllText(studentFile, "");
            }

            System.IO.File.AppendAllText(studentFile, $"{model.FullName},{model.Email},{model.Password}\n");
            return RedirectToAction("Login", "Login");
        }
    }
}
