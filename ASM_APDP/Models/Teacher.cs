using System.IO;

namespace ASM_APDP.Models
{
    public class Teacher
    {

        //public string FullName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }

        //// ✅ Constructor mặc định để tránh lỗi CS8618
        //public Teacher()
        //{
        //    FullName = string.Empty;
        //    Email = string.Empty;
        //    Password = string.Empty;
        //}

        //// ✅ Lưu thông tin vào Teacher.csv
        //public static void SaveTeacher(string fullName, string email, string password)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

        //    // Kiểm tra nếu email đã tồn tại
        //    if (IsEmailExist(email))
        //    {
        //        throw new Exception("Email đã được đăng ký, vui lòng chọn email khác.");
        //    }

        //    // Nếu file chưa tồn tại, tạo file mới với tiêu đề
        //    if (!File.Exists(filePath))
        //    {
        //        File.WriteAllText(filePath, "FullName,Email,Password\n");
        //    }

        //    // Ghi dữ liệu vào file
        //    using (StreamWriter writer = new StreamWriter(filePath, true))
        //    {
        //        writer.WriteLine($"{fullName},{email},{password}");
        //    }
        //}

        //// ✅ Kiểm tra email có tồn tại hay không
        //public static bool IsEmailExist(string email)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

        //    if (File.Exists(filePath))
        //    {
        //        foreach (var line in File.ReadLines(filePath))
        //        {
        //            var data = line.Split(',');
        //            if (data.Length == 3 && data[1] == email)
        //            {
        //                return true; // Email đã tồn tại
        //            }
        //        }
        //    }
        //    return false; // Email chưa tồn tại
        //}

        //// ✅ Lấy thông tin giáo viên theo email và password
        //public static Teacher? GetTeacherByUsername(string email, string password)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

        //    if (File.Exists(filePath))
        //    {
        //        foreach (var line in File.ReadLines(filePath))
        //        {
        //            var data = line.Split(',');
        //            if (data.Length == 3 && data[1] == email && data[2] == password)
        //            {
        //                return new Teacher { FullName = data[0], Email = data[1], Password = data[2] };
        //            }
        //        }
        //    }

        //    return null; // Nếu không tìm thấy tài khoản hợp lệ
        //}


        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        public string DOB { get; set; }

        public string Address { get; set; } // Thêm thuộc tính Address vào lớp Teacher

        // ✅ Constructor mặc định để tránh lỗi CS8618

        public Teacher()
        {
            FullName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            PhoneNumber = string.Empty;
            Subject = string.Empty;
            DOB = string.Empty;
            Address = string.Empty; // Khởi tạo Address
        }

        public static string FilePath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Teacher.csv");

        public static void SaveTeacher(string fullName, string email, string password)
        {
            if (IsEmailExist(email))
            {
                throw new Exception("Email đã được đăng ký, vui lòng chọn email khác.");
            }

            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "FullName,Email,Password,PhoneNumber,Subject,DOB\n");
            }

            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine($"{fullName},{email},{password},,,"); // default thêm cột mới rỗng
            }
        }

        public static bool IsEmailExist(string email)
        {
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath).Skip(1))
                {
                    var data = line.Split(',');
                    if (data.Length >= 3 && data[1] == email)
                        return true;
                }
            }
            return false;
        }

        public static Teacher? GetTeacherByUsername(string email, string password)
        {
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath).Skip(1))
                {
                    var data = line.Split(',');
                    if (data.Length >= 3 && data[1] == email && data[2] == password)
                    {
                        return new Teacher
                        {
                            FullName = data[0],
                            Email = data[1],
                            Password = data[2],
                            PhoneNumber = data.Length > 3 ? data[3] : "",
                            Subject = data.Length > 4 ? data[4] : "",
                            DOB = data.Length > 5 ? data[5] : "",
                            Address = data.Length > 6 ? data[6] : "" // Thêm Address vào đây
                        };
                    }
                }
            }

            return null;
        }

        public static void UpdateTeacherProfile(Teacher updatedTeacher)
        {
            var lines = File.ReadAllLines(FilePath).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                var data = lines[i].Split(',');
                if (data.Length >= 3 && data[1] == updatedTeacher.Email)
                {
                    lines[i] = $"{updatedTeacher.FullName},{updatedTeacher.Email},{updatedTeacher.Password},{updatedTeacher.PhoneNumber},{updatedTeacher.Subject},{updatedTeacher.DOB},{updatedTeacher.Address}";
                    break;
                }
            }

            File.WriteAllLines(FilePath, lines);
        }
    }
}
