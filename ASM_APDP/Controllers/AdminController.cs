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

    // Handle creating a new teacher account
    [HttpPost]
    public IActionResult CreateTeacherAccount(string fullName, string email, string password)
    {
        // Check if email already exists
        if (Teacher.IsEmailExist(email))
        {
            // If email exists, display error message
            ViewBag.ErrorMessage = "❌ Email already exists. Please choose a different email.";
            return View("CreateAccountTeacher");
        }

        // If email does not exist, save the teacher account
        Teacher.SaveTeacher(fullName, email, password);

        // Display success message
        ViewBag.SuccessMessage = "✅ New account created successfully!";
        return View("CreateAccountTeacher");
    }
}
