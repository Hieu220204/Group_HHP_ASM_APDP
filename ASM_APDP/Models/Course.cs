using System;
using System.Collections.Generic;
using System.IO;

namespace ASM_APDP.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }

        private static string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Course.csv");

        public static List<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            if (!File.Exists(FilePath)) return courses;

            foreach (var line in File.ReadLines(FilePath).Skip(1))
            {
                var data = line.Split(',');
                if (data.Length == 6)
                {
                    courses.Add(new Course
                    {
                        Id = int.Parse(data[0]),
                        Name = data[1],
                        Description = data[2],
                        TeacherName = data[3],
                        Duration = data[4],
                        Status = data[5]
                    });
                }
            }
            return courses;
        }

        public static void AddCourse(string name, string desc, string teacher, string duration, string status)
        {
            var courses = GetAllCourses();
            int newId = courses.Any() ? courses.Max(c => c.Id) + 1 : 1;

            using (var writer = new StreamWriter(FilePath, !File.Exists(FilePath)))
            {
                if (writer.BaseStream.Length == 0)
                    writer.WriteLine("Id,Name,Description,TeacherName,Duration,Status");

                writer.WriteLine($"{newId},{name},{desc},{teacher},{duration},{status}");
            }
        }

        public static Course GetCourseById(int id)
        {
            return GetAllCourses().FirstOrDefault(c => c.Id == id);
        }

        public static void UpdateCourse(int id, string name, string desc, string teacher, string duration, string status)
        {
            var courses = GetAllCourses();
            var updatedCourses = courses.Select(c =>
            {
                if (c.Id == id)
                {
                    c.Name = name;
                    c.Description = desc;
                    c.TeacherName = teacher;
                    c.Duration = duration;
                    c.Status = status;
                }
                return c;
            }).ToList();

            SaveAllCourses(updatedCourses);
        }

        public static void DeleteCourse(int id)
        {
            var courses = GetAllCourses().Where(c => c.Id != id).ToList();
            SaveAllCourses(courses);
        }

        private static void SaveAllCourses(List<Course> courses)
        {
            using (var writer = new StreamWriter(FilePath, false))
            {
                writer.WriteLine("Id,Name,Description,TeacherName,Duration,Status");
                foreach (var c in courses)
                {
                    writer.WriteLine($"{c.Id},{c.Name},{c.Description},{c.TeacherName},{c.Duration},{c.Status}");
                }
            }
        }
    }

}
