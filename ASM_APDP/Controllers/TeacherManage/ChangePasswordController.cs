using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;


namespace ASM_APDP.Controllers.TeacherManage
{
    public class ChangePasswordController : Controller
    {
        // Hiển thị trang đổi mật khẩu
        public IActionResult ChangePasswordTeacher()
        {
            return View("~/Views/Teacher/ChangePasswordTeacher.cshtml");
        }

        [HttpPost]
        public IActionResult ChangePasswordTeacher(string email, string oldPassword, string newPassword, string confirmPassword)
        {
            var teacher = Teacher.GetTeacherByEmail(email, oldPassword);

            if (teacher == null)
            {
                ViewBag.ErrorMessage = "Account not found.";
                return View("~/Views/Teacher/ChangePasswordTeacher.cshtml");
            }

            if (teacher.Password != oldPassword)
            {
                ViewBag.ErrorMessage = "Old password is incorrect.";
                return View("~/Views/Teacher/ChangePasswordTeacher.cshtml");
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "New passwords do not match.";
                return View("~/Views/Teacher/ChangePasswordTeacher.cshtml");
            }

            bool isUpdated = Teacher.UpdatePassword(email, newPassword);

            if (isUpdated)
            {
                ViewBag.SuccessMessage = "Password has been successfully changed!";
            }
            else
            {
                ViewBag.ErrorMessage = "An error occurred while changing the password.";
            }

            return View("~/Views/Teacher/ChangePasswordTeacher.cshtml");
        }
    }
}
