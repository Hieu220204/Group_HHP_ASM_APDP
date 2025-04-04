using System.IO;

namespace ASM_APDP.Models
{
    public class Teacher
    {

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // ✅ Constructor mặc định để tránh lỗi CS8618
        public Teacher()
        {
            FullName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        // ✅ Lưu thông tin vào Teacher.csv
        public static void SaveTeacher(string fullName, string email, string password)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

            // Kiểm tra nếu email đã tồn tại
            if (IsEmailExist(email))
            {
                throw new Exception("Email đã được đăng ký, vui lòng chọn email khác.");
            }

            // Nếu file chưa tồn tại, tạo file mới với tiêu đề
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "FullName,Email,Password\n");
            }

            // Ghi dữ liệu vào file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{fullName},{email},{password}");
            }
        }

        // ✅ Kiểm tra email có tồn tại hay không
        public static bool IsEmailExist(string email)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    var data = line.Split(',');
                    if (data.Length == 3 && data[1] == email)
                    {
                        return true; // Email đã tồn tại
                    }
                }
            }
            return false; // Email chưa tồn tại
        }

        // ✅ Lấy thông tin giáo viên theo email và password
        public static Teacher? GetTeacherByUsername(string email, string password)
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

            return null; // Nếu không tìm thấy tài khoản hợp lệ
        }
    }
}
