using Microsoft.AspNetCore.Mvc;
using System.IO;
using ASM_APDP.Models;

public class TeacherController : Controller
{
    // Trang TeacherHome sau khi Teacher đăng nhập
    public IActionResult TeacherHome()
    {
        return View();
    }

    // Phương thức xử lý đăng nhập của Teacher
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Kiểm tra thông tin đăng nhập của Teacher
        var teacher = Teacher.GetTeacherByUsername(email, password);
        if (teacher != null)
        {
            return RedirectToAction("TeacherHome", "Teacher");
        }

        ViewBag.Error = "Invalid login credentials.";
        return View("Login");
    }
}
