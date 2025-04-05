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

    // GET: Manage Course
    public IActionResult ManageCourse()
    {
        var courses = Course.GetAllCourses();
        return View(courses);
    }

    [HttpPost]
    public IActionResult AddCourse(string Name, string Description, string TeacherName, string Duration, string Status)
    {
        Course.AddCourse(Name, Description, TeacherName, Duration, Status);
        return RedirectToAction("ManageCourse");
    }

    public IActionResult EditCourse(int id)
    {
        var course = Course.GetCourseById(id);
        var allCourses = Course.GetAllCourses();
        ViewBag.SelectedCourse = course;
        return View("ManageCourse", allCourses);
    }

    [HttpPost]
    public IActionResult EditCourse(int id, string Name, string Description, string TeacherName, string Duration, string Status)
    {
        Course.UpdateCourse(id, Name, Description, TeacherName, Duration, Status);
        return RedirectToAction("ManageCourse");
    }

    [HttpPost]
    public IActionResult DeleteCourse(int id)
    {
        Course.DeleteCourse(id);
        return RedirectToAction("ManageCourse");
    }

    // Hiển thị trang quản lý môn học
    // Manage Subject Page
    public IActionResult ManageSubject()
    {
        var subjects = Subject.GetAllSubjects();
        return View(subjects);
    }

    // Add Subject
    [HttpPost]
    public IActionResult AddSubject(Subject subject)
    {
        // Add the new subject to the CSV
        Subject.AddSubject(subject);
        // Redirect back to the ManageSubject page
        return RedirectToAction("ManageSubject");
    }

    // Edit Subject Page (For getting subject details to show in input fields)
    public IActionResult EditSubject(string subjectCode)
    {
        // Get the subject based on the provided SubjectCode
        var subject = Subject.GetSubjectByCode(subjectCode);
        if (subject != null)
        {
            // Send the subject data to the view
            ViewBag.SelectedSubject = subject;
            return View("ManageSubject", Subject.GetAllSubjects()); // Return to ManageSubject with updated subject list
        }
        return RedirectToAction("ManageSubject");
    }

    // Edit Subject Action (For updating the subject)
    [HttpPost]
    public IActionResult EditSubject(string subjectCode, Subject updatedSubject)
    {
        // Update the subject in the CSV based on the SubjectCode
        var success = Subject.UpdateSubject(subjectCode, updatedSubject);
        if (success)
        {
            // After successful update, return to ManageSubject
            return RedirectToAction("ManageSubject");
        }

        // If update failed, return the same page with updated model data
        return View("ManageSubject", Subject.GetAllSubjects());
    }

    // Delete Subject Action
    [HttpPost]
    public IActionResult DeleteSubject(string subjectCode)
    {
        // Delete the subject based on SubjectCode
        var success = Subject.DeleteSubject(subjectCode);
        return RedirectToAction("ManageSubject");
    }

    // Manage Schedule Page
    public IActionResult ManageSchedule()
    {
        var schedules = Schedule.GetAllSchedules();
        return View(schedules);
    }

    // Add Schedule
    [HttpPost]
    public IActionResult AddSchedule(Schedule schedule)
    {
        Schedule.AddSchedule(schedule);
        return RedirectToAction("ManageSchedule");
    }

    // Edit Schedule Page (For getting schedule details to show in input fields)
    public IActionResult EditSchedule(string className)
    {
        // Lấy thông tin lịch học cần sửa
        var schedule = Schedule.GetScheduleByClassName(className);
        if (schedule != null)
        {
            // Hiển thị dữ liệu hiện tại để sửa
            ViewBag.SelectedSchedule = schedule;
            return View("ManageSchedule", Schedule.GetAllSchedules());
        }
        return RedirectToAction("ManageSchedule");
    }

    // Edit Schedule Action (For updating the schedule)
    [HttpPost]
    public IActionResult EditSchedule(string className, Schedule updatedSchedule)
    {
        // Kiểm tra dữ liệu đã nhập
        if (ModelState.IsValid)
        {
            // Gọi phương thức UpdateSchedule để cập nhật
            var success = Schedule.UpdateSchedule(className, updatedSchedule);
            if (success)
            {
                // Sau khi cập nhật, chuyển về trang ManageSchedule
                return RedirectToAction("ManageSchedule");
            }
            else
            {
                // Nếu update không thành công, hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "❌ Failed to update the schedule.";
            }
        }
        return View("ManageSchedule", Schedule.GetAllSchedules());
    }

    // Delete Schedule Action
    [HttpPost]
    public IActionResult DeleteSchedule(string className)
    {
        var success = Schedule.DeleteSchedule(className);
        return RedirectToAction("ManageSchedule");
    }
}
