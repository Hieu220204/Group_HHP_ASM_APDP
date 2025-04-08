using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASM_APDP.Models;

namespace ASM_APDP.Services
{
    public class CourseService
    {
        //private static CourseService _instance;

        //// Prevent instance creation from outside the class
        //private CourseService() { }

        //public CourseService getInstance()
        //{
        //    // If there is no instance, create a new one.
        //    if (_instance == null)
        //    {
        //        _instance = new CourseService();
        //    }
        //    // Returns instance (if already exists, reuse)
        //    return _instance;
        //}

        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Course.csv");

        // Example method to get all courses
        public List<Course> GetAllCourses()
        {
            var courses = new List<Course>();

            if (File.Exists(_filePath))
            {
                var lines = File.ReadAllLines(_filePath);
                foreach (var line in lines.Skip(1)) // Skip header row
                {
                    var columns = line.Split(',');
                    var course = new Course
                    {
                        Id = int.Parse(columns[0]),
                        Name = columns[1],
                        Description = columns[2],
                        TeacherName = columns[3],
                        Duration = columns[4],
                        Status = columns[5]
                    };
                    courses.Add(course);
                }
            }

            return courses;
        }

        // Example method to get a course by ID
        public Course GetCourseById(int id)
        {
            var courses = GetAllCourses();
            return courses.FirstOrDefault(c => c.Id == id);
        }

        // Example method to add a new course
        public void AddCourse(string name, string description, string teacherName, string duration, string status)
        {
            var courses = GetAllCourses();
            var newCourse = new Course
            {
                Id = courses.Count > 0 ? courses.Max(c => c.Id) + 1 : 1, // Ensure new ID is unique
                Name = name,
                Description = description,
                TeacherName = teacherName,
                Duration = duration,
                Status = status
            };

            // Append to the file
            using (var writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine($"{newCourse.Id},{newCourse.Name},{newCourse.Description},{newCourse.TeacherName},{newCourse.Duration},{newCourse.Status}");
            }
        }

        // Example method to update an existing course
        public void UpdateCourse(int id, string name, string description, string teacherName, string duration, string status)
        {
            var courses = GetAllCourses();
            var course = courses.FirstOrDefault(c => c.Id == id);

            if (course != null)
            {
                course.Name = name;
                course.Description = description;
                course.TeacherName = teacherName;
                course.Duration = duration;
                course.Status = status;

                // Write all courses back to file, including the updated one
                using (var writer = new StreamWriter(_filePath))
                {
                    writer.WriteLine("Id,Name,Description,TeacherName,Duration,Status"); // Write header
                    foreach (var c in courses)
                    {
                        writer.WriteLine($"{c.Id},{c.Name},{c.Description},{c.TeacherName},{c.Duration},{c.Status}");
                    }
                }
            }
        }

        // Example method to delete a course
        public void DeleteCourse(int id)
        {
            var courses = GetAllCourses();
            var courseToDelete = courses.FirstOrDefault(c => c.Id == id);

            if (courseToDelete != null)
            {
                courses.Remove(courseToDelete);

                // Write remaining courses back to the file
                using (var writer = new StreamWriter(_filePath))
                {
                    writer.WriteLine("Id,Name,Description,TeacherName,Duration,Status"); // Write header
                    foreach (var c in courses)
                    {
                        writer.WriteLine($"{c.Id},{c.Name},{c.Description},{c.TeacherName},{c.Duration},{c.Status}");
                    }
                }
            }
        }
    }
}
