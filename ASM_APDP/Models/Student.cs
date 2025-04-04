using System.IO;

namespace ASM_APDP.Models
{
    public class Student
    {
        //public string FullName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }

        //// Lưu thông tin vào Student.csv
        //public static void SaveStudent(string fullName, string email, string password)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

        //    if (IsEmailExist(email))
        //    {
        //        throw new Exception("Email đã được đăng ký, vui lòng chọn email khác.");
        //    }

        //    if (!File.Exists(filePath))
        //    {
        //        File.WriteAllText(filePath, "FullName,Email,Password\n");
        //    }

        //    using (StreamWriter writer = new StreamWriter(filePath, true))
        //    {
        //        writer.WriteLine($"{fullName},{email},{password}");
        //    }
        //}

        //// Kiểm tra email đã tồn tại hay chưa
        //public static bool IsEmailExist(string email)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

        //    if (File.Exists(filePath))
        //    {
        //        foreach (var line in File.ReadLines(filePath))
        //        {
        //            var data = line.Split(',');
        //            if (data.Length == 3 && data[1] == email)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //// Lấy thông tin sinh viên theo email và mật khẩu
        //public static Student GetStudentByEmail(string email, string password)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

        //    if (File.Exists(filePath))
        //    {
        //        foreach (var line in File.ReadLines(filePath))
        //        {
        //            var data = line.Split(',');
        //            if (data.Length == 3 && data[1] == email && data[2] == password)
        //            {
        //                return new Student { FullName = data[0], Email = data[1], Password = data[2] };
        //            }
        //        }
        //    }

        //    return null; // Nếu không tìm thấy sinh viên hợp lệ
        //}


        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }   // Thêm trường PhoneNumber
        public string DOB { get; set; }            // Thêm trường Date of Birth (DOB)
        public string Address { get; set; }        // Thêm trường Address

        // Lưu thông tin vào Student.csv
        public static void SaveStudent(string fullName, string email, string password, string phoneNumber, string dob, string address)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (IsEmailExist(email))
            {
                throw new Exception("Email đã được đăng ký, vui lòng chọn email khác.");
            }

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "FullName,Email,Password,PhoneNumber,DOB,Address\n");
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{fullName},{email},{password},{phoneNumber},{dob},{address}");
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
                    if (data.Length == 6 && data[1] == email) // Chỉnh sửa để kiểm tra 6 trường dữ liệu
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
                    if (data.Length == 6 && data[1] == email && data[2] == password) // Sửa lại để phù hợp với số trường
                    {
                        return new Student
                        {
                            FullName = data[0],
                            Email = data[1],
                            Password = data[2],
                            PhoneNumber = data[3], // Lấy thông tin PhoneNumber
                            DOB = data[4],          // Lấy thông tin DOB
                            Address = data[5]       // Lấy thông tin Address
                        };
                    }
                }
            }

            return null; // Nếu không tìm thấy sinh viên hợp lệ
        }

        // Cập nhật thông tin sinh viên
        public static void UpdateStudent(string email, string fullName, string password, string phoneNumber, string dob, string address)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");
            var lines = File.ReadAllLines(filePath).ToList();

            for (int i = 1; i < lines.Count; i++) // Bỏ qua dòng tiêu đề
            {
                var data = lines[i].Split(',');
                if (data[1] == email)
                {
                    lines[i] = $"{fullName},{email},{password},{phoneNumber},{dob},{address}";
                    File.WriteAllLines(filePath, lines);
                    return;
                }
            }

            throw new Exception("Không tìm thấy sinh viên với email này.");
        }



    }
}
