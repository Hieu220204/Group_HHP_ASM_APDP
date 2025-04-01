using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASM_APDP.Services
{
    public class AuthManagement
    {
        private readonly string adminFile = "Data/Admin.csv";
        private readonly string teacherFile = "Data/Teacher.csv";
        private readonly string studentFile = "Data/Student.csv";

        public AuthManagement()
        {
            // Đảm bảo thư mục Data tồn tại
            Directory.CreateDirectory("Data");

            // Tạo file nếu chưa tồn tại
            if (!File.Exists(adminFile)) File.WriteAllText(adminFile, "Email,Password,Role\n");
            if (!File.Exists(teacherFile)) File.WriteAllText(teacherFile, "Email,Password,Role\n");
            if (!File.Exists(studentFile)) File.WriteAllText(studentFile, "FullName,Email,Password,Role\n");
        }

        // Xác thực đăng nhập Admin
        public bool ValidateAdmin(string email, string password)
        {
            return ValidateUser(email, password, adminFile);
        }

        // Xác thực đăng nhập Teacher
        public bool ValidateTeacher(string email, string password)
        {
            return ValidateUser(email, password, teacherFile);
        }

        // Xác thực đăng nhập Student
        public bool ValidateStudent(string email, string password)
        {
            return ValidateUser(email, password, studentFile);
        }

        // Hàm chung kiểm tra đăng nhập từ file CSV
        private bool ValidateUser(string email, string password, string filePath)
        {
            if (!File.Exists(filePath)) return false;

            var users = File.ReadAllLines(filePath)
                            .Skip(1) // Bỏ qua dòng tiêu đề
                            .Select(line => line.Split(','))
                            .Where(fields => fields.Length >= 3 && fields[1] == email && fields[2] == password)
                            .ToList();

            return users.Any();
        }

        // Đăng ký tài khoản Student
        public void RegisterStudent(string fullName, string email, string password)
        {
            File.AppendAllText(studentFile, $"{fullName},{email},{password},Student\n");
        }

        // Tạo tài khoản Teacher
        public void CreateTeacherAccount(string email, string password)
        {
            File.AppendAllText(teacherFile, $"{email},{password},Teacher\n");

        }

        // Tạo tài khoản Admin
        public void CreateAdminAccount(string email, string password)
        {
            File.AppendAllText(adminFile, $"{email},{password},Admin\n");
        }
    }
}
