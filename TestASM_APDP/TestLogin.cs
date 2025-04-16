using Xunit;
using ASM_APDP.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ASM_APDP.Models;
using ASM_APDP.Controllers.StudentManage;
using Microsoft.AspNetCore.Http;

namespace TestProject
{
    public class LoginControllerTests
    {
        private LoginController GetControllerWithSession()
        {
            var context = new DefaultHttpContext();

            // Mock session
            var sessionMock = new Mock<ISession>();
            var sessionStorage = new Dictionary<string, byte[]>();

            sessionMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                       .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

            sessionMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                       .Returns((string key, out byte[] value) =>
                       {
                           return sessionStorage.TryGetValue(key, out value);
                       });

            context.Session = sessionMock.Object;

            // Tạo controller và gán context
            var controller = new LoginController(null) // vì constructor không cần service
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = context
                }
            };

            return controller;
        }

        // Mock AuthenticationService
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockAuthService = new Mock<IAuthenticationService>();
            _controller = new LoginController(_mockAuthService.Object);
        }

        [Fact]
        public void Login_ValidAdminCredentials_RedirectsToAdminHome()
        {
            // Arrange
            string email = "admin@gmail.com";
            string password = "admin123";
            _mockAuthService.Setup(service => service.Authenticate(email, password))
                .Returns(new AdminController());  // Return a mocked Admin object

            // Act
            var result = _controller.Login(email, password) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AdminHome", result.ActionName);
            Assert.Equal("Admin", result.ControllerName);
        }

        [Fact]
        public void Login_ValidStudentCredentials_RedirectsToStudentHome()
        {
            var controller = GetControllerWithSession();

            // Gán danh sách giả lập
            StudentManagement.Students = new List<Student>
            {
                new Student { Email = "student1@gmail.com", Password = "pass123" }
            };

            // Gọi phương thức login
            var result = controller.Login("student1@gmail.com", "pass123") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("StudentHome", result.ActionName);
            Assert.Equal("Student", result.ControllerName);
        }


        [Fact]
        public void Login_InvalidCredentials_ReturnsViewWithError()
        {
            // Arrange
            string email = "invalid@gmail.com";
            string password = "wrongpass";
            _mockAuthService.Setup(service => service.Authenticate(email, password))
                .Returns((object)null);  // Return null for invalid credentials

            // Act
            var result = _controller.Login(email, password) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid login information. Please check your email and password again.", _controller.ViewBag.Error);
        }
    }
}
