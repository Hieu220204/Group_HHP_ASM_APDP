using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using ASM_APDP.Models;

public class AttendanceController : Controller
{
    private readonly string csvPath = "App_Data/attendance.csv";

    public IActionResult Index()
    {
        var attendances = ReadAttendanceFromCSV();
        return View(attendances);
    }

    private List<Attendance> ReadAttendanceFromCSV()
    {
        var attendanceList = new List<Attendance>();

        if (!System.IO.File.Exists(csvPath)) return attendanceList;

        var lines = System.IO.File.ReadAllLines(csvPath);
        foreach (var line in lines.Skip(1)) // Bỏ dòng tiêu đề
        {
            var parts = line.Split(',');

            if (parts.Length == 5)
            {
                attendanceList.Add(new Attendance
                {
                    AttendanceID = parts[0],
                    StudentID = parts[1],
                    CourseID = parts[2],
                    Date = DateTime.Parse(parts[3], CultureInfo.InvariantCulture),
                    Status = parts[4]
                });
            }
        }

        return attendanceList;
    }
}
