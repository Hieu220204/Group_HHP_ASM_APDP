using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASM_APDP.Models;

namespace ASM_APDP.Services
{
    public class GradeService
    {
        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Grades.csv");

        // Ensure the Grades CSV file exists
        public void EnsureFileExists()
        {
            if (!File.Exists(filePath))
            {
                var header = "FullName,Email,Subject,Score,Notes";
                File.WriteAllText(filePath, header + Environment.NewLine);
            }
        }

        // Get all grades from the file
        public List<Grade> GetAllGrades()
        {
            var grades = new List<Grade>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1))
                {
                    var columns = line.Split(',');
                    if (columns.Length == 5)
                    {
                        if (double.TryParse(columns[3], out double score))
                        {
                            grades.Add(new Grade
                            {
                                FullName = columns[0],
                                Email = columns[1],
                                Subject = columns[2],
                                Score = score,
                                Notes = columns[4]
                            });
                        }
                    }
                }
            }
            return grades;
        }


        // Add a grade to the file
        public void AddGrade(Grade grade)
        {
            var newLine = $"{grade.FullName},{grade.Email},{grade.Subject},{grade.Score},{grade.Notes}";
            try
            {
                File.AppendAllText(filePath, newLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to file: " + ex.Message);
            }
        }

        // Get a grade by key (FullName, Email, and Subject)
        public Grade GetGradeByKey(string fullName, string email, string subject)
        {
            return GetAllGrades().FirstOrDefault(g =>
                g.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase) &&
                g.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                g.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
        }

        // Update a grade in the file
        public bool UpdateGrade(Grade updatedGrade)
        {
            var grades = GetAllGrades();
            var grade = grades.FirstOrDefault(g =>
                g.FullName == updatedGrade.FullName &&
                g.Email == updatedGrade.Email &&
                g.Subject == updatedGrade.Subject);

            if (grade != null)
            {
                grade.Score = updatedGrade.Score;
                grade.Notes = updatedGrade.Notes;

                File.WriteAllLines(filePath,
                    new[] { "FullName,Email,Subject,Score,Notes" }.Concat(
                        grades.Select(g => $"{g.FullName},{g.Email},{g.Subject},{g.Score},{g.Notes}")
                    ));
                return true;
            }
            return false;
        }

        // Delete a grade from the file
        public bool DeleteGrade(string fullName, string email, string subject)
        {
            var grades = GetAllGrades();
            var grade = grades.FirstOrDefault(g =>
                g.FullName == fullName &&
                g.Email == email &&
                g.Subject == subject);

            if (grade != null)
            {
                grades.Remove(grade);
                File.WriteAllLines(filePath,
                    new[] { "FullName,Email,Subject,Score,Notes" }.Concat(
                        grades.Select(g => $"{g.FullName},{g.Email},{g.Subject},{g.Score},{g.Notes}")
                    ));
                return true;
            }
            return false;
        }


    }
}
