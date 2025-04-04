using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models; 

public class AdminController : Controller
{
    public IActionResult AdminHome()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }

    public IActionResult CreateAccountTeacher()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateTeacherAccount(string fullName, string email, string password)
    {
        Teacher.SaveTeacher(fullName, email, password);

        ViewBag.SuccessMessage = "New account added successfully!";

        return View("CreateAccountTeacher");
    }


    private readonly string filePath = "wwwroot/Data/Schedule.csv";

    private readonly IWebHostEnvironment _env;

    public AdminController(IWebHostEnvironment env)
    {
        _env = env;
        Subject.Initialize(env); // Load dữ liệu từ Subject.csv khi controller được tạo
        Course.Initialize(env); // Load dữ liệu từ Course.csv khi controller được tạo
    }


    // Manage course
    public IActionResult ManageCourse()
    {
        return View(Course.Courses);
    }

    [HttpPost]
    public IActionResult AddCourse(string name)
    {
        Course.AddCourse(name);
        return RedirectToAction("ManageCourse");
    }

    [HttpPost]
    public IActionResult EditCourse(int id, string newName)
    {
        Course.EditCourse(id, newName);
        return RedirectToAction("ManageCourse");
    }

    [HttpPost]
    public IActionResult DeleteCourse(int id)
    {
        Course.DeleteCourse(id);
        return RedirectToAction("ManageCourse");
    }

    // Manage subject
    public IActionResult ManageSubject()
    {
        ViewBag.Courses = Course.Courses;
        return View(Subject.Subjects);
    }

    [HttpPost]
    public IActionResult AddSubject(string name, int courseId)
    {
        Subject.AddSubject(name, courseId);
        return RedirectToAction("ManageSubject");
    }

    [HttpPost]
    public IActionResult EditSubject(int id, string newName, int newCourseId)
    {
        Subject.EditSubject(id, newName, newCourseId);
        return RedirectToAction("ManageSubject");
    }

    [HttpPost]
    public IActionResult DeleteSubject(int id)
    {
        Subject.DeleteSubject(id);
        return RedirectToAction("ManageSubject");
    }


    

    // Đọc dữ liệu từ Schedule.csv
    private List<Schedule> ReadSchedules()
    {
        var schedules = new List<Schedule>();

        if (!System.IO.File.Exists(filePath))
            return schedules;

        var lines = System.IO.File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 4)
            {
                schedules.Add(new Schedule
                {
                    Id = schedules.Count + 1,
                    SubjectName = parts[0],
                    TeacherName = parts[1],
                    Date = parts[2],
                    Time = parts[3]
                });
            }
        }
        return schedules;
    }

    // Lưu danh sách vào Schedule.csv
    private void SaveSchedules(List<Schedule> schedules)
    {
        var lines = schedules.Select(s => $"{s.SubjectName},{s.TeacherName},{s.Date},{s.Time}").ToList();
        System.IO.File.WriteAllLines(filePath, lines);
    }

    // Hiển thị danh sách lịch học
    public IActionResult ManageSchedule()
    {
        var schedules = ReadSchedules();
        return View(schedules);
    }

    // Thêm lịch học mới
    [HttpPost]
    public IActionResult AddSchedule(string subjectName, string teacherName, string date, string time)
    {
        var schedules = ReadSchedules();
        schedules.Add(new Schedule
        {
            Id = schedules.Count + 1,
            SubjectName = subjectName,
            TeacherName = teacherName,
            Date = date,
            Time = time
        });
        SaveSchedules(schedules);
        return RedirectToAction("ManageSchedule");
    }

    // Xóa lịch học
    [HttpPost]
    public IActionResult DeleteSchedule(int id)
    {
        var schedules = ReadSchedules();
        schedules.RemoveAll(s => s.Id == id);
        SaveSchedules(schedules);
        return RedirectToAction("ManageSchedule");
    }

    // Chỉnh sửa lịch học
    [HttpPost]
    public IActionResult EditSchedule(int id, string subjectName, string teacherName, string date, string time)
    {
        var schedules = ReadSchedules();
        var schedule = schedules.FirstOrDefault(s => s.Id == id);
        if (schedule != null)
        {
            schedule.SubjectName = subjectName;
            schedule.TeacherName = teacherName;
            schedule.Date = date;
            schedule.Time = time;
            SaveSchedules(schedules);
        }
        return RedirectToAction("ManageSchedule");
    }
}
