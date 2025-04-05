using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using Microsoft.AspNetCore.Http;

public class TeacherController : Controller
{
    public IActionResult TeacherHome()
    {
        return View();
    }

    public IActionResult ChangePasswordTeacher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ChangePasswordTeacher(string email, string oldPassword, string newPassword, string confirmPassword)
    {
        var teacher = Teacher.GetTeacherByEmail(email, oldPassword);

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

        bool isUpdated = Teacher.UpdatePassword(email, newPassword);

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

    public IActionResult ManageGrade()
    {
        Grade.EnsureFileExists();
        var grades = Grade.GetAllGrades();
        return View(grades);
    }

    [HttpPost]
    public IActionResult AddGrade(Grade grade)
    {
        if (ModelState.IsValid)
        {
            Grade.AddGrade(grade);
            return RedirectToAction("ManageGrade");
        }
        return View("ManageGrade", Grade.GetAllGrades());
    }

    public IActionResult EditGrade(string fullName, string email, string subject)
    {
        var grade = Grade.GetGradeByKey(fullName, email, subject);
        if (grade != null)
        {
            return View(grade);
        }
        return RedirectToAction("ManageGrade");
    }

    [HttpPost]
    public IActionResult EditGrade(Grade updatedGrade)
    {
        var success = Grade.UpdateGrade(updatedGrade);
        if (success)
        {
            TempData["SuccessMessage"] = "Grade updated successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to update grade.";
        }

        return RedirectToAction("ManageGrade");
    }

    [HttpPost]
    public IActionResult DeleteGrade(string fullName, string email, string subject)
    {
        var success = Grade.DeleteGrade(fullName, email, subject);
        if (success)
        {
            TempData["SuccessMessage"] = "Grade deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete grade.";
        }

        return RedirectToAction("ManageGrade");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }
}
