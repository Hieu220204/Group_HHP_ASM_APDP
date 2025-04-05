using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;

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
}
