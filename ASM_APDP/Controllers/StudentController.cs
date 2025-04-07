using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;

public class StudentController : Controller
{
    // Trang chủ của sinh viên
    public IActionResult StudentHome()
    {
        var email = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Login", "Auth");

        var student = Student.GetStudentByEmailOnly(email);

        if (student != null)
            return View(student);

        return RedirectToAction("Login", "Auth");
    }

    // Trang xem thông tin cá nhân
    public IActionResult ViewProfile()
    {
        var email = HttpContext.Session.GetString("UserEmail");  // Lấy email từ phiên làm việc

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Auth");
        }

        var student = Student.GetStudentByEmailOnly(email);  // Lấy sinh viên từ email

        if (student != null)
        {
            return View(student);  // Trả về View với thông tin sinh viên
        }

        return RedirectToAction("Login", "Auth");  // Nếu không tìm thấy, chuyển hướng đến trang đăng nhập
    }

    // Cập nhật thông tin cá nhân
    [HttpPost]
    public IActionResult UpdateProfile(string fullName, string phoneNumber)
    {
        var email = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Auth");
        }

        var students = Student.GetAllStudents();
        var student = students.FirstOrDefault(s => s.Email == email);

        if (student != null)
        {
            student.FullName = fullName;
            student.PhoneNumber = phoneNumber;

            Student.SaveStudents(students);  // Lưu lại toàn bộ danh sách

            ViewBag.SuccessMessage = "✅ Cập nhật thông tin thành công!";
            return View("ViewProfile", student);
        }

        return RedirectToAction("Login", "Auth");
    }
}
