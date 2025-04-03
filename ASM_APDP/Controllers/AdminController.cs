using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models; 

public class AdminController : Controller
{
    public IActionResult AdminHome()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }

    public IActionResult CreateAccountTeacher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateTeacherAccount(string fullName, string email, string password)
    {
        Teacher.SaveTeacher(fullName, email, password);

        ViewBag.SuccessMessage = "Thêm mới tài khoản thành công";

        return View("CreateAccountTeacher");
    }
}
