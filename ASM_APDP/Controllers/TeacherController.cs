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

    public IActionResult ManageGrade()
    {
        return View(); // Đảm bảo có file View tương ứng
    }

    public IActionResult ManageAttendance()
    {
        return View(); // Đảm bảo có file View tương ứng
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


    // Action to show Manage Grades page
    public IActionResult ManageGrade()
    {
        var grades = Grade.GetGrades(); // Lấy dữ liệu điểm từ cơ sở dữ liệu
        return View(grades); // Trả về dữ liệu điểm cho View
    }

    // Action to show Manage Attendance page
    public IActionResult ManageAttendance()
    {
        var attendanceRecords = Attendance.GetAttendance(); // Lấy dữ liệu điểm danh từ cơ sở dữ liệu
        return View(attendanceRecords); // Trả về dữ liệu điểm danh cho View
    }

    // Handle adding a new grade
    [HttpPost]
    public IActionResult AddGrade(string studentID, string courseID, string grade)
    {
        bool isAdded = Grade.AddGrade(studentID, courseID, grade); // Thêm mới điểm vào cơ sở dữ liệu
        if (isAdded)
        {
            ViewBag.SuccessMessage = "Grade has been added successfully!";
        }
        else
        {
            ViewBag.ErrorMessage = "An error occurred while adding the grade.";
        }
        return RedirectToAction("ManageGrades");
    }

    // Handle adding a new attendance record
    [HttpPost]
    public IActionResult AddAttendance(string studentID, string courseID, DateTime date, string status)
    {
        bool isAdded = Attendance.AddAttendance(studentID, courseID, date, status); // Thêm mới điểm danh vào cơ sở dữ liệu
        if (isAdded)
        {
            ViewBag.SuccessMessage = "Attendance record has been added successfully!";
        }
        else
        {
            ViewBag.ErrorMessage = "An error occurred while adding the attendance record.";
        }
        return RedirectToAction("ManageAttendance");
    }

    // Action to change the teacher's password
    public IActionResult ChangePasswordTeacher()
    {
        return View(); // Render ChangePasswordTeacher view
    }

    // Action to log out
    public IActionResult Logout()
    {
        // Clear session data to log out
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth"); // Redirect to the login page
    }
}
