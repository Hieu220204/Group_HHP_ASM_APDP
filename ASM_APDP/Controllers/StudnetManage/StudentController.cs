using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services;
using ASM_APDP.Controllers.StudentManage;

public class StudentController : Controller
{
    // Trang chủ của sinh viên
    public IActionResult StudentHome()
    {
        return View();
    }

    // Trang xem thông tin cá nhân
    public IActionResult Profile()
    {
        var email = HttpContext.Session.GetString("UserEmail"); // Lấy email từ session
        var student = StudentManagement.GetStudentProfile(email); // Lấy thông tin sinh viên từ CSV

        if (student == null)
        {
            return RedirectToAction("Login", "Auth"); // Nếu không tìm thấy sinh viên trong session, chuyển hướng đến trang login
        }

        return View(student); // Trả về view Profile với thông tin sinh viên
    }

    // Cập nhật thông tin cá nhân
    [HttpPost]
    public IActionResult Profile(string fullName, string dateOfBirth, string phoneNumber, string hometown, string major)
    {
        var email = HttpContext.Session.GetString("UserEmail"); // Lấy email từ session
        var student = StudentManagement.GetStudentProfile(email); // Lấy thông tin sinh viên từ CSV

        if (student == null)
        {
            return RedirectToAction("Login", "Auth"); // Nếu không tìm thấy sinh viên trong session, chuyển hướng đến trang login
        }

        // Cập nhật thông tin sinh viên
        bool isUpdated = StudentManagement.UpdateStudentInfo(email, fullName, dateOfBirth, phoneNumber, hometown, major);

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
    [HttpPost]
    public IActionResult ChangePasswordStudent(string email, string oldPassword, string newPassword, string confirmPassword)
    {
        // Kiểm tra thông tin sinh viên qua email và mật khẩu cũ
        var student = StudentManagement.GetStudentByEmail(email, oldPassword);

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
        bool isUpdated = StudentManagement.UpdatePassword(email, newPassword);

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


    private readonly ScheduleService _scheduleService;

    // Constructor để inject ScheduleService
    public StudentController(ScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    // Action để xem lịch học của học sinh
    public IActionResult ViewSchedule()
    {
        // Lấy tất cả lịch học từ ScheduleService
        var schedules = _scheduleService.GetAllSchedules();

        // Trả về view và truyền dữ liệu lịch học
        return View("~/Views/Student/ViewSchedule.cshtml", schedules);
    }
}
