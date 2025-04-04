using ASM_APDP.Models;
using Microsoft.AspNetCore.Mvc;

public class StudentController : Controller
{
    public IActionResult StudentHome()
    {
        return View();
    }

    // Phương thức đọc dữ liệu từ Schedule.csv
    private List<Schedule> ReadSchedule()
    {
        string filePath = "wwwroot/Data/Schedule.csv";
        List<Schedule> schedules = new List<Schedule>();

        if (System.IO.File.Exists(filePath))
        {
            var lines = System.IO.File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1)) // Bỏ qua dòng tiêu đề
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    schedules.Add(new Schedule
                    {
                        SubjectName = parts[0],
                        TeacherName = parts[1],
                        Date = parts[2],
                        Time = parts[3]
                    });
                }
            }
        }
        return schedules;
    }

    // Hiển thị danh sách lịch học của giáo viên hiện tại
    public IActionResult ViewSchedule()
    {
        var schedules = ReadSchedule();
        return View(schedules);
    }

    // Action hiển thị trang cập nhật profile
    public IActionResult UpdateProfile()
    {
        // Lấy thông tin sinh viên hiện tại từ session hoặc một cơ chế xác thực khác
        var currentStudent = GetCurrentStudent(); // Hàm này cần phải trả về đối tượng sinh viên hiện tại

        if (currentStudent == null)
        {
            return RedirectToAction("Login", "Auth"); // Nếu không tìm thấy sinh viên, chuyển hướng đến trang đăng nhập
        }

        return View(currentStudent); // Truyền thông tin sinh viên hiện tại vào view
    }

    // Action xử lý việc cập nhật thông tin sinh viên
    [HttpPost]
    public IActionResult UpdateProfile(Student updatedStudent)
    {
        try
        {
            // Cập nhật thông tin sinh viên vào CSV
            Student.UpdateStudent(updatedStudent.Email, updatedStudent.FullName, updatedStudent.Password, updatedStudent.PhoneNumber, updatedStudent.DOB, updatedStudent.Address);
            ViewBag.Message = "Thông tin đã được cập nhật thành công.";
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Có lỗi xảy ra: {ex.Message}";
        }

        return View(updatedStudent); // Trả về lại trang cập nhật thông tin
    }

    // Hàm lấy thông tin sinh viên từ session hoặc hệ thống xác thực (bạn cần implement lại)
    private Student GetCurrentStudent()
    {
        // Thực hiện lấy thông tin sinh viên từ session hoặc cookie
        // Ví dụ giả sử lấy thông tin từ email đã đăng nhập:
        var email = User.Identity.Name; // Hoặc từ session/cookie khác
        var password = "somePassword";  // Thực tế, bạn phải lấy mật khẩu từ session nếu có
        return Student.GetStudentByEmail(email, password);
    }



}



