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

            // Lưu thông tin sinh viên vào CSV file
            Student.SaveStudent(fullName, email, password, dob, numberPhone, hometown, major);

            // Kiểm tra xem dữ liệu có được lưu thành công không
            if (Student.IsEmailExist(email))
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
