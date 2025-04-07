using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASM_APDP.Models
{
    public class Student
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Hometown { get; set; }
        public string Major { get; set; }

        // Lấy tất cả sinh viên từ CSV
        public static List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            var filePath = "wwwroot/Data/Students.csv"; // Đường dẫn tới tệp CSV lưu sinh viên

            // Đảm bảo tệp tồn tại
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath); // Đọc các dòng từ tệp CSV

                foreach (var line in lines.Skip(1))  // Bỏ qua dòng tiêu đề
                {
                    var values = line.Split(',');
                    students.Add(new Student
                    {
                        FullName = values[0],
                        Email = values[1],
                        Password = values[2],
                        DateOfBirth = values[3],
                        PhoneNumber = values[4],
                        Hometown = values[5],
                        Major = values[6]

                    });
                }
            }

            return students;
        }

        // Lưu danh sách sinh viên vào tệp CSV
        public static void SaveStudents(List<Student> students)
        {
            var filePath = "wwwroot/Data/Students.csv";
            var lines = new List<string> { "FullName,Email,Password,DateOfBirth,PhoneNumber,Hometown,Major" }; // Tiêu đề CSV

            // Thêm các sinh viên vào danh sách dòng
            foreach (var student in students)
            {
                lines.Add($"{student.FullName},{student.Email},{student.Password},{student.DateOfBirth},{student.PhoneNumber},{student.Hometown},{student.Major}");
            }

            // Ghi các dòng vào tệp CSV
            File.WriteAllLines(filePath, lines);
        }

        // Kiểm tra nếu email đã tồn tại trong danh sách sinh viên
        public static bool IsEmailExist(string email)
        {
            var students = GetAllStudents();  // Lấy tất cả sinh viên
            return students.Any(s => s.Email == email);  // Kiểm tra email có tồn tại không
        }

        // Lấy sinh viên theo email và mật khẩu
        public static Student GetStudentByEmail(string email, string password)
        {
            var students = GetAllStudents();  // Lấy tất cả sinh viên
            return students.FirstOrDefault(s => s.Email == email && s.Password == password);  // Tìm sinh viên theo email và mật khẩu
        }

        internal static object GetStudentByEmail(string? email)
        {
            throw new NotImplementedException();
        }
    }
}
