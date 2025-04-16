using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services; // Import the GradeService

namespace ASM_APDP.Controllers.TeacherManage
{
    public class GradeController : Controller
    {
        private readonly GradeService _gradeService;

        // Constructor to inject the GradeService
        public GradeController(GradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // Hiển thị trang quản lý điểm
        public IActionResult ManageGrade()
        {
            _gradeService.EnsureFileExists();
            var grades = _gradeService.GetAllGrades();  // Using _gradeService to get all grades
            return View("~/Views/Teacher/ManageGrade.cshtml", grades);
        }

        [HttpPost]
        public IActionResult AddGrade(Grade grade)
        {
            if (ModelState.IsValid)
            {
                _gradeService.AddGrade(grade);
                return RedirectToAction("ManageGrade");
            }

            // Nếu có lỗi trong model, hiển thị lại trang với danh sách grade hiện tại
            return View("~/Views/Teacher/ManageGrade.cshtml", _gradeService.GetAllGrades());  // Using _gradeService to get all grades
        }

        // Hiển thị trang chỉnh sửa điểm
        public IActionResult EditGrade(string fullName, string email, string subject)
        {
            var grade = _gradeService.GetGradeByKey(fullName, email, subject);
            if (grade != null)
            {
                return View(grade);
            }
            return RedirectToAction("ManageGrade");
        }

        [HttpPost]
        public IActionResult EditGrade(Grade updatedGrade)
        {
            var success = _gradeService.UpdateGrade(updatedGrade);
            if (success)
            {
                TempData["SuccessMessage"] = "Grade updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update grade.";
            }

            return RedirectToAction("ManageGrade");
        }

        [HttpPost]
        public IActionResult DeleteGrade(string fullName, string email, string subject)
        {
            var success = _gradeService.DeleteGrade(fullName, email, subject);
            if (success)
            {
                TempData["SuccessMessage"] = "Grade deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete grade.";
            }

            return RedirectToAction("ManageGrade");
        }

        public IActionResult ViewGrade(string studentEmail)
        {
            var grades = _gradeService.GetAllGrades()
                .Where(g => g.Email.Equals(studentEmail, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (grades.Count == 0)
            {
                TempData["ErrorMessage"] = "No grades found for this student.";
            }

            return View("~/Views/Student/ViewGrade.cshtml", grades);
        }






    }
}
