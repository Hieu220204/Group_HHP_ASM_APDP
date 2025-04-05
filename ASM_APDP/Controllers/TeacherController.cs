using Microsoft.AspNetCore.Mvc;
using System.IO;
using ASM_APDP.Models;

public class TeacherController : Controller
{
    // Trang TeacherHome sau khi Teacher đăng nhập
    public IActionResult TeacherHome()
    {
        return View();
    }

    // Phương thức xử lý đăng nhập của Teacher
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Kiểm tra thông tin đăng nhập của Teacher
        var teacher = Teacher.GetTeacherByUsername(email, password);
        if (teacher != null)
        {
            return RedirectToAction("TeacherHome", "Teacher");
        }

        ViewBag.Error = "Invalid login credentials.";
        return View("Login");
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


    public IActionResult UpdateProfile()
    {
        // (Tạm thời giả lập lấy thông tin giáo viên đang đăng nhập từ session hoặc cookie)
        string email = "teacher1@gmail.com"; // TODO: Thay bằng email từ session/cookie
        string password = "123";             // Tương tự như trên

        var teacher = Teacher.GetTeacherByUsername(email, password);
        if (teacher != null)
        {
            return View(teacher);
        }

        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult UpdateProfile(Teacher updatedTeacher)
    {
        Teacher.UpdateTeacherProfile(updatedTeacher);
        ViewBag.Message = "Profile updated successfully!";
        return View(updatedTeacher);
    }



}