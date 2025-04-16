using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using Microsoft.AspNetCore.Http;

public class TeacherController : Controller
{
    public IActionResult TeacherHome()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }
}
