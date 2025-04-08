using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASM_APDP.Models;

namespace ASM_APDP.Services
{
    public class ScheduleService
    {
        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Schedule.csv");

        // Get all schedules
        public List<Schedule> GetAllSchedules()
        {
            var schedules = new List<Schedule>();
            if (!File.Exists(filePath)) return schedules;

            foreach (var line in File.ReadLines(filePath).Skip(1)) // Skip header line
            {
                var data = line.Split(',');
                if (data.Length == 6)
                {
                    schedules.Add(new Schedule
                    {
                        CourseName = data[0].Trim(),
                        ClassName = data[1].Trim(),
                        Teacher = data[2].Trim(),
                        Date = DateTime.TryParse(data[3], out DateTime parsedDate) ? parsedDate : DateTime.MinValue,
                        Time = data[4].Trim(),
                        Status = data[5].Trim()
                    });
                }
            }
            return schedules;
        }

        // Add a schedule
        public void AddSchedule(Schedule schedule)
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
                // Consider using a logging framework to log the error
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        // Get schedule by class name
        public Schedule GetScheduleByClassName(string className)
        {
            var schedules = GetAllSchedules();
            return schedules.FirstOrDefault(s => s.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase));
        }

        // Update schedule
        public bool UpdateSchedule(string className, Schedule updatedSchedule)
        {
            var schedules = GetAllSchedules();
            var scheduleToUpdate = schedules.FirstOrDefault(s => s.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase));

            if (scheduleToUpdate != null)
            {
                scheduleToUpdate.CourseName = updatedSchedule.CourseName;
                scheduleToUpdate.Teacher = updatedSchedule.Teacher;
                scheduleToUpdate.Date = updatedSchedule.Date;
                scheduleToUpdate.Time = updatedSchedule.Time;
                scheduleToUpdate.Status = updatedSchedule.Status;

                // Write updated schedules back to the file
                try
                {
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating schedule: {ex.Message}");
                    return false;
                }
            }
            return false; // Return false if schedule not found
        }

        // Delete schedule
        public bool DeleteSchedule(string className)
        {
            var schedules = GetAllSchedules();
            var scheduleToDelete = schedules.FirstOrDefault(s => s.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase));
            if (scheduleToDelete != null)
            {
                schedules.Remove(scheduleToDelete);
                try
                {
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting schedule: {ex.Message}");
                    return false;
                }
            }
            return false; // Return false if schedule not found
        }
    }
}
