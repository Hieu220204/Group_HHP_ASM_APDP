using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;

namespace ASM_APDP.Controllers.AdminManage
{
    public class TeacherAccountController : Controller
    {
        // Hiển thị form tạo tài khoản giáo viên
        public IActionResult Create()
        {
            // Update to use the correct view location
            return View("~/Views/Admin/CreateAccountTeacher.cshtml");  // Full path to the view
        }

        // Xử lý tạo tài khoản giáo viên
        [HttpPost]
        public IActionResult Create(string fullName, string email, string password)
        {
            if (Teacher.IsEmailExist(email))
            {
                ViewBag.ErrorMessage = "❌ Email already exists. Please choose a different email.";
                return View("~/Views/Admin/CreateAccountTeacher.cshtml");  // Full path to the view
            }

            Teacher.SaveTeacher(fullName, email, password);
            ViewBag.SuccessMessage = "✅ New account created successfully!";
            return View("~/Views/Admin/CreateAccountTeacher.cshtml");  // Full path to the view
        }
    }
}