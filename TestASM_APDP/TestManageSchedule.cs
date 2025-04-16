using Xunit;
using System.IO;
using ASM_APDP.Models;
using ASM_APDP.Services;
using System;
using System.Linq;

namespace TestASM_APDP
{
    public class TestManageSchedule
    {
        private readonly string schedulePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Schedule.csv");

        private void PrepareScheduleCsvWithHeader()
        {
            // Đảm bảo file CSV có header hợp lệ
            Directory.CreateDirectory(Path.GetDirectoryName(schedulePath));
            File.WriteAllText(schedulePath, "CourseName,ClassName,Teacher,Date,Time,Status\n");
        }

        [Fact]
        public void AddSchedule_ShouldAppendScheduleToFile()
        {
            // Arrange
            PrepareScheduleCsvWithHeader(); // Làm sạch dữ liệu cũ
            var service = new ScheduleService();

            var schedule = new Schedule
            {
                CourseName = "Test Course",
                ClassName = "CL01",
                Teacher = "Teacher A",
                Date = DateTime.Today,
                Time = "10:00",
                Status = "Planned"
            };

            // Act
            service.AddSchedule(schedule);

            // Assert
            var result = service.GetAllSchedules();
            var added = result.FirstOrDefault(s => s.ClassName == "CL01");
            Assert.NotNull(added);
            Assert.Equal("Test Course", added.CourseName);
        }

        [Fact]
        public void GetScheduleByClassName_ShouldReturnCorrectSchedule()
        {
            // Arrange
            PrepareScheduleCsvWithHeader();
            File.AppendAllText(schedulePath, "Math,CL02,Mr. John,2025-04-20,09:00,Confirmed\n");

            var service = new ScheduleService();

            // Act
            var schedule = service.GetScheduleByClassName("CL02");

            // Assert
            Assert.NotNull(schedule);
            Assert.Equal("Math", schedule.CourseName);
        }

        // Tương tự bạn có thể viết các test cho Update và Delete
    }
}
