using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models; 

public class LoginController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        if (email == "admin@gmail.com" && password == "admin123")
        {
            return RedirectToAction("AdminHome", "Admin"); 
        }

        var student = Student.GetStudentByEmail(email, password);
        if (student != null)
        {
            return RedirectToAction("StudentHome", "Student");
        }

        var teacher = Teacher.GetTeacherByUsername(email, password);
        if (teacher != null)
        {
            return RedirectToAction("TeacherHome", "Teacher");
        }

        ViewBag.Error = "Invalid login information. Please check your email and password again.";
        return View();
    }
}
