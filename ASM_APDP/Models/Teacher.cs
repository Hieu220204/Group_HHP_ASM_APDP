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
    }
}
