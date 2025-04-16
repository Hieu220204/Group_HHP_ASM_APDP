using ASM_APDP.Controllers.StudentManage;
using ASM_APDP.Models;
using Microsoft.AspNetCore.Mvc;

public class RegisterController : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RegisterStudent(string fullName, string email, string password, string dob, string numberPhone, string hometown, string major)
    {
        try
        {
            // Kiểm tra email đã tồn tại chưa
            if (StudentManagement.IsEmailExist(email))
            {
                ViewBag.ErrorMessage = "This email has already been registered. Please choose a different email.";
                return View("Register");  // Trả về view Register khi email trùng
            }

            // Lưu thông tin sinh viên vào CSV file (using SaveStudents)
            var students = StudentManagement.GetAllStudents();  // Lấy danh sách sinh viên hiện có
            students.Add(new Student
            {
                FullName = fullName,
                Email = email,
                Password = password,
                DateOfBirth = dob,
                PhoneNumber = numberPhone,
                Hometown = hometown,
                Major = major
            });

            // Lưu lại danh sách sinh viên vào CSV
            StudentManagement.SaveStudents(students);

            // Kiểm tra xem dữ liệu có được lưu thành công không
            if (StudentManagement.IsEmailExist(email))  // Đảm bảo email đã được lưu
            {
                TempData["SuccessMessage"] = "Registration successful! You can log in now.";
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewBag.ErrorMessage = "An error occurred, the account could not be saved!";
                return View("Register");  // Trả về view Register khi có lỗi
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "An error occurred while registering the account: " + ex.Message;
            return View("Register");  // Trả về view Register nếu có ngoại lệ
        }
    }
}
