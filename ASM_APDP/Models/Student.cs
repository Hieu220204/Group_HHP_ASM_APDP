using System.IO;

namespace ASM_APDP.Models
{
    public class Student
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Lưu thông tin vào Student.csv
        public static void SaveStudent(string fullName, string email, string password)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (IsEmailExist(email))
            {
                throw new Exception("Email đã được đăng ký, vui lòng chọn email khác.");
            }

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "FullName,Email,Password\n");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{fullName},{email},{password}");
            }
        }

        // Kiểm tra email đã tồn tại hay chưa
        public static bool IsEmailExist(string email)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    var data = line.Split(',');
                    if (data.Length == 3 && data[1] == email)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Lấy thông tin sinh viên theo email và mật khẩu
        public static Student GetStudentByEmail(string email, string password)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    var data = line.Split(',');
                    if (data.Length == 3 && data[1] == email && data[2] == password)
                    {
                        return new Student { FullName = data[0], Email = data[1], Password = data[2] };
                    }
                }
            }

            return null; // Nếu không tìm thấy sinh viên hợp lệ
        }
    }
}
