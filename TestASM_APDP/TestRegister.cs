using ASM_APDP.Controllers.StudentManage;
using ASM_APDP.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace TestASM_APDP
{
    public class TestRegister
    {
        private string tempDirectory;
        private string tempFilePath;

        public TestRegister()
        {
            // Tạo thư mục và đường dẫn tạm cho file CSV
            tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data");
            tempFilePath = Path.Combine(tempDirectory, "Students.csv");

            // Đảm bảo thư mục tồn tại
            Directory.CreateDirectory(tempDirectory);
        }

        // Cleanup: Xóa tệp và thư mục tạm sau khi test
        ~TestRegister()
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            if (Directory.Exists(tempDirectory) && Directory.GetFiles(tempDirectory).Length == 0)
            {
                Directory.Delete(tempDirectory);
            }
        }

        // Kiểm thử đăng ký sinh viên với email mới chưa tồn tại
        [Fact]
        public void RegisterStudent_NewEmail_ReturnsRedirectToLogin()
        {
            // Arrange
            string fullName = "Test User";
            string email = "testuser@example.com";
            string password = "Test123";
            string dob = "2000-01-01";
            string numberPhone = "1234567890";
            string hometown = "Hanoi";
            string major = "IT";

            // Clean file trước
            var filePath = StudentManagement.GetFilePath();
            File.WriteAllText(filePath, "FullName,Email,Password,DateOfBirth,PhoneNumber,Hometown,Major\n");

            var controller = new RegisterController();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.RegisterStudent(fullName, email, password, dob, numberPhone, hometown, major);

            // Assert
            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
        }

        // Kiểm thử đăng ký sinh viên với email đã tồn tại
        [Fact]
        public void RegisterStudent_DuplicateEmail_ReturnsViewWithError()
        {
            // Arrange
            var controller = new RegisterController();
            string fullName = "Nguyen Thi B";
            string email = "duplicate@example.com";  // Sử dụng email trùng lặp
            string password = "654321";
            string dob = "02/02/2001";
            string phone = "0987654321";
            string hometown = "Da Nang";
            string major = "Economics";

            // Tạo file CSV với một sinh viên đã có email
            File.WriteAllText(StudentManagement.GetFilePath(), "FullName,Email,Password,DateOfBirth,PhoneNumber,Hometown,Major\n");
            var existingStudent = new Student
            {
                FullName = "Existing Student",
                Email = "duplicate@example.com", // Email trùng lặp
                Password = "password123",
                DateOfBirth = "01/01/2000",
                PhoneNumber = "123456789",
                Hometown = "Ho Chi Minh City",
                Major = "Computer Science"
            };

            // Lưu vào file CSV
            var students = new List<Student> { existingStudent };
            StudentManagement.SaveStudents(students);

            // Act
            var result = controller.RegisterStudent(fullName, email, password, dob, phone, hometown, major) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Register", result.ViewName);

            string errorMessage = result.ViewData["ErrorMessage"]?.ToString();
            Assert.NotNull(errorMessage);
            Console.WriteLine("Actual error message: " + errorMessage);

            Assert.Contains("This email has already been registered.", errorMessage);
        }
    }
}
