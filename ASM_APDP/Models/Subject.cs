using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace ASM_APDP.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }

        private static string FilePath;
        private static int nextId = 1;

        public static void Initialize(IWebHostEnvironment env)
        {
            FilePath = Path.Combine(env.WebRootPath, "Data", "Subject.csv");
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "Id,Name,CourseId\n");
            }
            LoadSubjects();
        }

        public static List<Subject> Subjects { get; private set; } = new List<Subject>();

        private static void LoadSubjects()
        {
            Subjects.Clear();
            var lines = File.ReadAllLines(FilePath).Skip(1);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 3 && int.TryParse(parts[0], out int id) && int.TryParse(parts[2], out int courseId))
                {
                    Subjects.Add(new Subject { Id = id, Name = parts[1], CourseId = courseId });
                    nextId = Math.Max(nextId, id + 1);
                }
            }
        }

        private static void SaveSubjects()
        {
            var lines = new List<string> { "Id,Name,CourseId" };
            lines.AddRange(Subjects.Select(s => $"{s.Id},{s.Name},{s.CourseId}"));
            File.WriteAllLines(FilePath, lines);
        }

        public static void AddSubject(string name, int courseId)
        {
            Subjects.Add(new Subject { Id = nextId++, Name = name, CourseId = courseId });
            SaveSubjects();
        }

        public static void EditSubject(int id, string newName, int newCourseId)
        {
            var subject = Subjects.FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                subject.Name = newName;
                subject.CourseId = newCourseId;
                SaveSubjects();
            }
        }

        public static void DeleteSubject(int id)
        {
            Subjects.RemoveAll(s => s.Id == id);
            SaveSubjects();
        }
    }
}
