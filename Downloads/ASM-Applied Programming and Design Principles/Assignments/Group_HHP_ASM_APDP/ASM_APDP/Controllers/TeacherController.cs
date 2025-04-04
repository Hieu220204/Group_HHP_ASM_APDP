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

    // Action to display ManageGrades page
    public IActionResult ManageGrades()
    {
        if (!IsTeacherAuthenticated()) return RedirectToAction("Login", "Auth");
        var grades = LoadGrades(); // Load grades from file
        return View(grades);
    }

    [HttpPost]
    public IActionResult AddGrade(string studentID, string courseID, string grade)
    {
        if (!IsTeacherAuthenticated()) return RedirectToAction("Login", "Auth");
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Grades.csv");
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{studentID},{courseID},{grade}");
        }
        return RedirectToAction("ManageGrades");
    }

    // Action to display ManageAttendance page
    public IActionResult ManageAttendance()
    {
        if (!IsTeacherAuthenticated()) return RedirectToAction("Login", "Auth");
        var attendanceRecords = LoadAttendance(); // Load attendance from file
        return View(attendanceRecords);
    }

    [HttpPost]
    public IActionResult AddAttendance(string studentID, string courseID, string date, string status)
    {
        if (!IsTeacherAuthenticated()) return RedirectToAction("Login", "Auth");
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Attendance.csv");
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{studentID},{courseID},{date},{status}");
        }
        return RedirectToAction("ManageAttendance");
    }

    private List<Grade> LoadGrades()
    {
        var grades = new List<Grade>();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Grades.csv");
        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var data = line.Split(',');
                if (data.Length == 3)
                {
                    grades.Add(new Grade { StudentID = data[0], CourseID = data[1], Score = data[2] });
                }
            }
        }
        return grades;
    }

    private List<Attendance> LoadAttendance()
    {
        var records = new List<Attendance>();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Attendance.csv");
        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var data = line.Split(',');
                if (data.Length == 4)
                {
                    records.Add(new Attendance { StudentID = data[0], CourseID = data[1], Date = data[2], Status = data[3] });
                }
            }
        }
        return records;
    }

    private bool IsTeacherAuthenticated()
    {
        var email = HttpContext.Session.GetString("TeacherEmail");
        if (string.IsNullOrEmpty(email)) return false;

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");
        if (!File.Exists(filePath)) return false;

        foreach (var line in File.ReadLines(filePath))
        {
            var data = line.Split(',');
            if (data.Length == 3 && data[1] == email)
            {
                return true;
            }
        }
        return false;
    }
}

public class Grade
{
    public string StudentID { get; set; }
    public string CourseID { get; set; }
    public string Score { get; set; }
}

public class Attendance
{
    public string StudentID { get; set; }
    public string CourseID { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
}