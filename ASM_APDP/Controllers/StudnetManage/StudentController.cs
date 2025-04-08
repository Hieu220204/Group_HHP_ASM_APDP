using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services;

public class StudentController : Controller
{
    // Trang chủ của sinh viên
    public IActionResult StudentHome()
    {
        return View();
    }

    // Trang xem thông tin cá nhân
    public IActionResult ViewProfile()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        var student = Student.GetStudentByEmail(email);  // Lấy thông tin sinh viên từ email

        // Kiểm tra nếu sinh viên không null và ép kiểu lại nếu cần
        if (student != null)
        {
            return View(student);  // Trả về view với thông tin sinh viên
        }

        return RedirectToAction("Login", "Auth");  // Nếu không tìm thấy, chuyển hướng đến trang đăng nhập
    }

    // Cập nhật thông tin cá nhân
    [HttpPost]
    public IActionResult UpdateProfile(string fullName, string phoneNumber)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        var student = Student.GetStudentByEmail(email);  // Lấy thông tin sinh viên từ email

        // Kiểm tra nếu sinh viên tồn tại và ép kiểu lại đối tượng student
        if (student != null)
        {
            

            // Cập nhật lại danh sách sinh viên sau khi chỉnh sửa thông tin
            Student.SaveStudents(Student.GetAllStudents());
            ViewBag.SuccessMessage = "✅ Cập nhật thông tin thành công!";
        }

        return View("ViewProfile", student);  // Trả về view "ViewProfile" với thông tin sinh viên đã cập nhật
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
