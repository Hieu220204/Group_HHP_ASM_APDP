using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASM_APDP.Models
{
    public class Schedule
    {
        public string CourseName { get; set; }
        public string ClassName { get; set; }
        public string Teacher { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }

        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Schedule.csv");

        // Get all schedules
        public static List<Schedule> GetAllSchedules()
        {
            var schedules = new List<Schedule>();
            if (!File.Exists(filePath)) return schedules;

            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                var data = line.Split(',');
                if (data.Length == 6)
                {
                    schedules.Add(new Schedule
                    {
                        CourseName = data[0],
                        ClassName = data[1],
                        Teacher = data[2],
                        Date = DateTime.Parse(data[3]),
                        Time = data[4],
                        Status = data[5]
                    });
                }
            }
            return schedules;
        }

        // Add a schedule
        public static void AddSchedule(Schedule schedule)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "CourseName,ClassName,Teacher,Date,Time,Status\n");
                }

                using (var writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{schedule.CourseName},{schedule.ClassName},{schedule.Teacher},{schedule.Date},{schedule.Time},{schedule.Status}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }


        // Get schedule by class name
        public static Schedule GetScheduleByClassName(string className)
        {
            var schedules = GetAllSchedules();
            return schedules.FirstOrDefault(s => s.ClassName == className);
        }

        // Update schedule
        public static bool UpdateSchedule(string className, Schedule updatedSchedule)
        {
            // Lấy tất cả các lịch học từ file CSV
            var schedules = GetAllSchedules();

            // Tìm lịch học cần cập nhật
            var scheduleToUpdate = schedules.FirstOrDefault(s => s.ClassName == className);

            if (scheduleToUpdate != null)
            {
                // Cập nhật các thông tin của Schedule
                scheduleToUpdate.CourseName = updatedSchedule.CourseName;
                scheduleToUpdate.Teacher = updatedSchedule.Teacher;
                scheduleToUpdate.Date = updatedSchedule.Date;
                scheduleToUpdate.Time = updatedSchedule.Time;
                scheduleToUpdate.Status = updatedSchedule.Status;

                // Sau khi cập nhật, ghi lại tất cả các thông tin vào file CSV
                using (var writer = new StreamWriter(filePath))
                {
                    // Ghi tiêu đề đầu tiên
                    writer.WriteLine("CourseName,ClassName,Teacher,Date,Time,Status");

                    // Ghi lại tất cả lịch học vào file
                    foreach (var schedule in schedules)
                    {
                        writer.WriteLine($"{schedule.CourseName},{schedule.ClassName},{schedule.Teacher},{schedule.Date},{schedule.Time},{schedule.Status}");
                    }
                }

                return true; // Cập nhật thành công
            }

            return false; // Không tìm thấy thông tin lịch học
        }




        // Delete schedule
        public static bool DeleteSchedule(string className)
        {
            var schedules = GetAllSchedules();
            var scheduleToDelete = schedules.FirstOrDefault(s => s.ClassName == className);
            if (scheduleToDelete != null)
            {
                schedules.Remove(scheduleToDelete);
                // Rewrite the CSV file without the deleted schedule
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("CourseName,ClassName,Teacher,Date,Time,Status");
                    foreach (var schedule in schedules)
                    {
                        writer.WriteLine($"{schedule.CourseName},{schedule.ClassName},{schedule.Teacher},{schedule.Date},{schedule.Time},{schedule.Status}");
                    }
                }
                return true;
            }
            return false;
        }
    }
}