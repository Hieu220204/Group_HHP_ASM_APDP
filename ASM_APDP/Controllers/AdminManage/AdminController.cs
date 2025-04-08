using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;

public class AdminController : Controller
{
    // Admin Home page
    public IActionResult AdminHome()
    {
        return View();
    }

    // Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear session to log out
        return RedirectToAction("Login", "Auth"); // Redirect to Login page
    }

    // Show the form to create a new teacher account
    public IActionResult CreateAccountTeacher()
    {
        return View();
    }



    
}