using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using System;

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
            if (Student.IsEmailExist(email))
            {
                ViewBag.ErrorMessage = "This email has already been registered. Please choose a different email.";
                return View();
            }

            // Lưu thông tin sinh viên vào CSV file (using SaveStudents)
            var students = Student.GetAllStudents();  // Get all existing students first
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

            // Save the updated list of students back to the CSV
            Student.SaveStudents(students);

            // Kiểm tra xem dữ liệu có được lưu thành công không
            if (Student.IsEmailExist(email))  // Ensure the email exists after saving
            {
                TempData["SuccessMessage"] = "Registration successful! You can log in now.";
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewBag.ErrorMessage = "An error occurred, the account could not be saved!";
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "An error occurred while registering the account: " + ex.Message;
            return View();
        }
    }


}
