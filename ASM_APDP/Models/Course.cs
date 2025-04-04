using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace ASM_APDP.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private static string FilePath;
        private static int nextId = 1;

        public static void Initialize(IWebHostEnvironment env)
        {
            FilePath = Path.Combine(env.WebRootPath, "Data", "Course.csv");
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "Id,Name\n");
            }
            LoadCourses();
        }

        public static List<Course> Courses { get; private set; } = new List<Course>();

        private static void LoadCourses()
        {
            Courses.Clear();
            var lines = File.ReadAllLines(FilePath).Skip(1); // Bỏ qua dòng tiêu đề
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                {
                    Courses.Add(new Course { Id = id, Name = parts[1] });
                    nextId = Math.Max(nextId, id + 1);
                }
            }
        }

        private static void SaveCourses()
        {
            var lines = new List<string> { "Id,Name" };
            lines.AddRange(Courses.Select(c => $"{c.Id},{c.Name}"));
            File.WriteAllLines(FilePath, lines);
        }

        public static void AddCourse(string name)
        {
            Courses.Add(new Course { Id = nextId++, Name = name });
            SaveCourses();
        }

        public static void EditCourse(int id, string newName)
        {
            var course = Courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                course.Name = newName;
                SaveCourses();
            }
        }

        public static void DeleteCourse(int id)
        {
            Courses.RemoveAll(c => c.Id == id);
            SaveCourses();
        }
    }
}
