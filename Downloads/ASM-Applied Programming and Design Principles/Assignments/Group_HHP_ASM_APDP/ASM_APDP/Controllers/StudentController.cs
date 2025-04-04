using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using System;

public class StudentController : Controller
{
    // Action to display the StudentHome page
    public IActionResult StudentHome()
    {
        return View(); // Ensure there's a StudentHome.cshtml in the Views/Student folder
    }

    // Action để hiển thị trang Profile
    public IActionResult Profile()
    {
        var email = HttpContext.Session.GetString("UserEmail"); // Lấy email từ session
        var student = Student.GetStudentProfile(email); // Lấy thông tin sinh viên từ CSV

        if (student == null)
        {
            return RedirectToAction("Login", "Auth"); // Nếu không tìm thấy sinh viên trong session, chuyển hướng đến trang login
        }

        return View(student); // Trả về view Profile với thông tin sinh viên
    }

    // Action để cập nhật thông tin Profile
    [HttpPost]
    [HttpPost]
    public IActionResult UpdateProfile(string fullName, string dateOfBirth, string phoneNumber, string hometown, string major)
    {
        var email = HttpContext.Session.GetString("UserEmail"); // Lấy email từ session
        var student = Student.GetStudentProfile(email); // Lấy thông tin sinh viên từ CSV

        if (student == null)
        {
            return RedirectToAction("Login", "Auth"); // Nếu không tìm thấy sinh viên trong session, chuyển hướng đến trang login
        }

        // Cập nhật thông tin sinh viên
        bool isUpdated = Student.UpdateStudentInfo(email, fullName, dateOfBirth, phoneNumber, hometown, major);

        if (isUpdated)
        {
            TempData["Message"] = "Profile updated successfully!"; // Lưu thông báo thành công vào TempData
        }
        else
        {
            TempData["Message"] = "Failed to update profile. Please try again."; // Lưu thông báo lỗi vào TempData
        }

        // Trả về lại trang Profile với thông báo
        return RedirectToAction("Profile");
    }

    // Action để hiển thị trang đổi mật khẩu
    public IActionResult ChangePasswordStudent()
    {
        return View();
    }

    // Xử lý yêu cầu thay đổi mật khẩu
    [HttpPost]
    public IActionResult ChangePasswordStudent(string email, string oldPassword, string newPassword, string confirmPassword)
    {
        // Kiểm tra thông tin sinh viên qua email và mật khẩu cũ
        var student = Student.GetStudentByEmail(email, oldPassword);

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

        // Cập nhật mật khẩu mới trong file CSV
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
