using ASM_APDP.Models;
using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
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

        // Kiểm tra đăng nhập của sinh viên
        var student = Student.GetStudentByEmail(email, password);
        if (student != null)
        {
            HttpContext.Session.SetString("UserEmail", email);
            return RedirectToAction("StudentHome", "Student");
        }

        // Kiểm tra đăng nhập của giáo viên
        var teacher = Teacher.GetTeacherByUsername(email, password);
        if (teacher != null)
        {
            return RedirectToAction("TeacherHome", "Teacher");
        }

        ViewBag.Error = "Invalid login credentials. Please try again.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login", "Auth");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string fullName, string email, string password)
    {
        if (Student.IsEmailExist(email))
        {
            ViewBag.ErrorMessage = "Email này đã được đăng ký. Vui lòng chọn email khác.";
            return View();
        }

        Student.SaveStudent(fullName, email, password);
        TempData["SuccessMessage"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";

        return View("Register");
    }
}
