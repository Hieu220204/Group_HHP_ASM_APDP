using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services;  // Ensure this namespace includes CourseService

namespace ASM_APDP.Controllers.AdminManage
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        // Constructor injection for CourseService
        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        // Hiển thị danh sách khoá học
        public IActionResult ManageCourse()
        {
            var courses = _courseService.GetAllCourses(); // Get all courses using the service
            return View("~/Views/Admin/ManageCourse.cshtml", courses); // Pass courses to the view
        }

        // Thêm mới khoá học (POST request)
        [HttpPost]
        public IActionResult Add(string Name, string Description, string TeacherName, string Duration, string Status)
        {
            _courseService.AddCourse(Name, Description, TeacherName, Duration, Status); // Add course using the service
            return RedirectToAction("ManageCourse"); // Redirect to the list of courses
        }

        // Load thông tin để sửa (GET request)
        public IActionResult Edit(int id)
        {
            var course = _courseService.GetCourseById(id); // Get course by ID using the service
            ViewBag.SelectedCourse = course; // Pass selected course to the view
            var courses = _courseService.GetAllCourses(); // Get all courses for display
            return View("~/Views/Admin/ManageCourse.cshtml", courses); // Return to the ManageCourse view
        }

        // Cập nhật khoá học sau khi chỉnh sửa (POST request)
        [HttpPost]
        public IActionResult Edit(int id, string Name, string Description, string TeacherName, string Duration, string Status)
        {
            _courseService.UpdateCourse(id, Name, Description, TeacherName, Duration, Status); // Update course using the service
            return RedirectToAction("ManageCourse"); // Redirect to the list of courses
        }

        // Xoá khoá học (POST request)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _courseService.DeleteCourse(id); // Delete course using the service
            return RedirectToAction("ManageCourse"); // Redirect to the list of courses
        }
    }
}
