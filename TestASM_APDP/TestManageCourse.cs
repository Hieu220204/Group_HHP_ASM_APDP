using Xunit;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Controllers.AdminManage;
using ASM_APDP.Services;
using ASM_APDP.Models;
using System.Linq;

namespace TestASM_APDP
{
    public class TestManageCourse
    {
        private void ResetData()
        {
            var lines = new List<string>
            {
                "Id,Name,Description,TeacherName,Duration,Status",
                "1,DSA101,Data Structures,John Doe,40 hours,Active",
                "2,WEB202,Web Programming,Jane Smith,30 hours,Inactive"
            };
            File.WriteAllLines(_filePath, lines);
        }

        private readonly string _filePath;
        private readonly CourseService _courseService;
        private readonly CourseController _controller;

        public TestManageCourse()
        {
            // Di chuyển lên thư mục gốc của solution từ bin/Debug/...
            var projectDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\ASM_APDP"));
            _filePath = Path.Combine(projectDir, "wwwroot", "Data", "Course.csv");

            _courseService = new CourseService(_filePath);
            _controller = new CourseController(_courseService);
        }

        [Fact]
        public void ManageCourse_ReturnsViewWithCourses()
        {
            var result = _controller.ManageCourse() as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as List<Course>;
            Assert.NotNull(model);
            Assert.True(model.Count >= 2); // hoặc số lượng mong muốn
        }

        [Fact]
        public void Add_AddsCourseAndRedirects()
        {
            var countBefore = _courseService.GetAllCourses().Count;

            var result = _controller.Add("WEB103", "Test Desc", "Teacher Test", "3 hours", "Active") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("ManageCourse", result.ActionName);

            var allCourses = _courseService.GetAllCourses();
            Assert.Equal(countBefore + 1, allCourses.Count);
            Assert.Contains(allCourses, c => c.Name == "WEB103");
        }

        [Fact]
        public void Edit_GET_ReturnsViewWithSelectedCourse()
        {
            ResetData();
            var result = _controller.Edit(1) as ViewResult;

            Assert.NotNull(result);

            // Sử dụng ViewData để lấy khóa học đã chọn
            var selected = result.ViewData["SelectedCourse"] as Course;
            Assert.NotNull(selected);
            Assert.Equal("DSA101", selected.Name); // Kiểm tra tên khóa học mong muốn
        }

        [Fact]
        public void Edit_POST_UpdatesCourseAndRedirects()
        {
            _controller.Edit(1, "UpdatedName", "Updated Desc", "New Teacher", "5 hours", "Inactive");
            var updated = _courseService.GetCourseById(1);

            Assert.Equal("UpdatedName", updated.Name);
            Assert.Equal("Inactive", updated.Status);
        }

        [Fact]
        public void Delete_RemovesCourseAndRedirects()
        {
            var courses = _courseService.GetAllCourses();
            if (!courses.Any()) return;

            int lastId = courses.Max(c => c.Id);
            _controller.Delete(lastId);

            var course = _courseService.GetCourseById(lastId);
            Assert.Null(course);
        }
    }
}
