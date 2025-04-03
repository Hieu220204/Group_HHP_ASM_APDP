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
    public IActionResult Register(string fullName, string email, string password)
    {
        try
        {
            if (Student.IsEmailExist(email))
            {
                ViewBag.ErrorMessage = "Email này đã được đăng ký. Vui lòng chọn email khác.";
                return View();
            }

            Student.SaveStudent(fullName, email, password);
            TempData["SuccessMessage"] = "Tạo tài khoản thành công! Bạn có thể đăng nhập ngay.";
            return View("Register"); // Giữ nguyên trang thay vì về Login
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Đã có lỗi khi đăng ký tài khoản: " + ex.Message;
            return View();
        }
    }
}
