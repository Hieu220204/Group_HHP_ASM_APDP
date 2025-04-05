using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;

public class AuthController : Controller
{
    // Trang đăng nhập
    public IActionResult Login()
    {
        return View();
    }

    // Xử lý yêu cầu đăng nhập (POST)
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Kiểm tra đăng nhập của Admin
        if (email == "admin@gmail.com" && password == "admin123")
        {
            return RedirectToAction("AdminHome", "Admin");
        }

        // Kiểm tra đăng nhập của sinh viên
        var student = Student.GetStudentByEmail(email, password);  // Phương thức này bây giờ nhận cả email và mật khẩu
        if (student != null)
        {
            // Lưu thông tin email của người dùng vào session
            HttpContext.Session.SetString("UserEmail", email);
            return RedirectToAction("StudentHome", "Student");
        }

        // Kiểm tra đăng nhập của giáo viên
        var teacher = Teacher.GetTeacherByEmail(email, password);  // Gọi phương thức GetTeacherByEmail
        if (teacher != null)
        {
            return RedirectToAction("TeacherHome", "Teacher");
        }

        // Nếu không tìm thấy tài khoản sinh viên hoặc giáo viên, hiển thị thông báo lỗi
        ViewBag.Error = "Thông tin đăng nhập không hợp lệ. Vui lòng thử lại.";
        return View();
    }

    // Hành động đăng xuất
    public IActionResult Logout()
    {
        // Xóa dữ liệu session khi đăng xuất
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");  // Chuyển hướng đến trang Đăng nhập
    }

    // Trang đăng ký
    public IActionResult Register()
    {
        return View();
    }

    // Xử lý yêu cầu đăng ký (POST)
    [HttpPost]
    public IActionResult Register(string fullName, string email, string password, string dob, string phoneNumber, string hometown, string major)
    {
        if (Student.IsEmailExist(email))  // Kiểm tra nếu email đã tồn tại
        {
            ViewBag.ErrorMessage = "❌ Email này đã được đăng ký. Vui lòng chọn email khác.";
            return View();
        }

        // Lưu sinh viên mới vào danh sách (sử dụng SaveStudents)
        var students = Student.GetAllStudents();  // Lấy danh sách tất cả sinh viên
        students.Add(new Student
        {
            FullName = fullName,
            Email = email,
            Password = password,
            DateOfBirth = dob,
            PhoneNumber = phoneNumber,
            Hometown = hometown,
            Major = major
        });

        // Lưu danh sách sinh viên đã cập nhật vào CSV
        Student.SaveStudents(students);  // Gọi SaveStudents để lưu danh sách đã thay đổi

        ViewBag.SuccessMessage = "✅ Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.";
        return View();
    }
}
