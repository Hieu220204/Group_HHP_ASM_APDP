using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using System;

public class StudentController : Controller
{
    // Action to display the StudentHome page
    public IActionResult StudentHome()
    {
        return View(); // Return the StudentHome view
    }

    // Display the change password page
    public IActionResult ChangePasswordStudent()
    {
        return View();
    }

    // Handle change password request
    [HttpPost]
    public IActionResult ChangePasswordStudent(string email, string oldPassword, string newPassword, string confirmPassword)
    {
        // Check student's information by email and old password
        var student = Student.GetStudentByEmail(email, oldPassword);  // Now checking using email as well

        if (student == null)
        {
            ViewBag.ErrorMessage = "Account not found.";
            return View();
        }

        if (student.Password != oldPassword)
        {
            ViewBag.ErrorMessage = "Old password is incorrect.";
            return View();
        }

        if (newPassword != confirmPassword)
        {
            ViewBag.ErrorMessage = "New passwords do not match.";
            return View();
        }

        // Update the new password in the CSV file
        bool isUpdated = Student.UpdatePassword(email, newPassword);

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

}
