using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;

public class TeacherController : Controller
{
    // Action to show TeacherHome page
    public IActionResult TeacherHome()
    {
        return View(); // Render TeacherHome view
    }

    // Action to display the ChangePasswordTeacher page
    public IActionResult ChangePasswordTeacher()
    {
        return View(); // Render ChangePasswordTeacher view
    }

    // Handle password change
    [HttpPost]
    public IActionResult ChangePasswordTeacher(string email, string oldPassword, string newPassword, string confirmPassword)
    {
        // Check teacher's credentials with email and old password
        var teacher = Teacher.GetTeacherByEmail(email, oldPassword); // Use GetTeacherByEmail method

        if (teacher == null)
        {
            ViewBag.ErrorMessage = "Account not found.";
            return View();
        }

        if (teacher.Password != oldPassword)
        {
            ViewBag.ErrorMessage = "Old password is incorrect.";
            return View();
        }

        if (newPassword != confirmPassword)
        {
            ViewBag.ErrorMessage = "New passwords do not match.";
            return View();
        }

        // Update the password in the system
        bool isUpdated = Teacher.UpdatePassword(email, newPassword); // Call method to update teacher's password

        if (isUpdated)
        {
            ViewBag.SuccessMessage = "Password has been successfully changed!";
        }
        else
        {
            ViewBag.ErrorMessage = "An error occurred while changing the password.";
        }

        return View();
    }

    // Action to log out
    public IActionResult Logout()
    {
        // Clear session data to log out
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth"); // Redirect to the login page
    }
}
