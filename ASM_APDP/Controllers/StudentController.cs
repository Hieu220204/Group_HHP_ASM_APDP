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
}
