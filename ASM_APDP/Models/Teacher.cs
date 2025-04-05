using System;
using System.IO;

namespace ASM_APDP.Models
{
    public class Teacher
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Save teacher information to Teacher.csv
        public static void SaveTeacher(string fullName, string email, string password)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

            // Check if the file exists, if not, create a new one
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "FullName,Email,Password\n");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{fullName},{email},{password}");
            }
        }

        // Check if the email already exists
        public static bool IsEmailExist(string email)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

            if (!File.Exists(filePath))
            {
                return false;
            }

            foreach (var line in File.ReadLines(filePath))
            {
                var data = line.Split(',');
                if (data.Length == 3 && data[1].Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // Get teacher information by email and password
        public static Teacher GetTeacherByEmail(string email, string password)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    var data = line.Split(',');
                    if (data.Length == 3 && data[1] == email && data[2] == password)
                    {
                        return new Teacher { FullName = data[0], Email = data[1], Password = data[2] };
                    }
                }
            }

            return null;
        }

        // Update the teacher's password
        public static bool UpdatePassword(string email, string newPassword)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");
            var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher_temp.csv");

            if (!File.Exists(filePath))
            {
                return false;
            }

            bool isUpdated = false;

            using (var reader = new StreamReader(filePath))
            using (var writer = new StreamWriter(tempFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(',');

                    if (data.Length == 3 && data[1] == email)
                    {
                        data[2] = newPassword; // Update the password
                        isUpdated = true;
                    }

                    writer.WriteLine(string.Join(",", data));
                }
            }

            // Replace the old file with the updated one
            if (isUpdated)
            {
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
            }

            return isUpdated;
        }

        // Lưu attendance vào file CSV
        public static void SaveAttendance(string studentID, string courseID, string date, string status)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Attendance.csv");

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "StudentID,CourseID,Date,Status\n");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{studentID},{courseID},{date},{status}");
            }
        }

        // Lấy tất cả attendance
        public static List<Attendance> GetAllAttendance()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Attendance.csv");
            var attendanceList = new List<Attendance>();

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath).Skip(1)) // Bỏ qua dòng đầu (header)
                {
                    var data = line.Split(',');
                    if (data.Length == 4)
                    {
                        attendanceList.Add(new Attendance
                        {
                            StudentID = data[0],
                            CourseID = data[1],
                            Date = data[2],
                            Status = data[3]
                        });
                    }
                }
            }

            return attendanceList;
        }

        // Lưu grade vào file CSV
        public static void SaveGrade(string studentID, string courseID, string score)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Grade.csv");

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "StudentID,CourseID,Score\n");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{studentID},{courseID},{score}");
            }
        }

        // Lấy tất cả grades
        public static List<Grade> GetAllGrades()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Grade.csv");
            var gradeList = new List<Grade>();

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath).Skip(1)) // Bỏ qua dòng đầu (header)
                {
                    var data = line.Split(',');
                    if (data.Length == 3)
                    {
                        gradeList.Add(new Grade
                        {
                            StudentID = data[0],
                            CourseID = data[1],
                            Score = data[2]
                        });
                    }
                }
            }

            return gradeList;
        }
    }

    // Class Attendance để lưu trữ thông tin vắng mặt
    public class Attendance
    {
        public string StudentID { get; set; }
        public string CourseID { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }

    // Class Grade để lưu trữ thông tin điểm
    public class Grade
    {
        public string StudentID { get; set; }
        public string CourseID { get; set; }
        public string Score { get; set; }
    }
}
}
}
