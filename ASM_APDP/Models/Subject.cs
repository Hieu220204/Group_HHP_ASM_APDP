using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASM_APDP.Models
{
    public class Subject
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public string Teacher { get; set; }
        public string Status { get; set; }

        //private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Subject.csv");

        //// Get all subjects from the CSV file
        //public static List<Subject> GetAllSubjects()
        //{
        //    var subjects = new List<Subject>();
        //    if (!File.Exists(filePath)) return subjects;

        //    foreach (var line in File.ReadLines(filePath).Skip(1))
        //    {
        //        var data = line.Split(',');
        //        if (data.Length == 5)
        //        {
        //            subjects.Add(new Subject
        //            {
        //                SubjectCode = data[0],
        //                SubjectName = data[1],
        //                Description = data[2],
        //                Teacher = data[3],
        //                Status = data[4]
        //            });
        //        }
        //    }
        //    return subjects;
        //}

        //// Add a new subject to the CSV file
        //public static void AddSubject(Subject subject)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        File.WriteAllText(filePath, "SubjectCode,SubjectName,Description,Teacher,Status\n");
        //    }

        //    using (var writer = new StreamWriter(filePath, true))
        //    {
        //        writer.WriteLine($"{subject.SubjectCode},{subject.SubjectName},{subject.Description},{subject.Teacher},{subject.Status}");
        //    }
        //}

        //// Get subject by its ID (SubjectCode)
        //public static Subject GetSubjectByCode(string subjectCode)
        //{
        //    var subjects = GetAllSubjects();
        //    return subjects.FirstOrDefault(s => s.SubjectCode == subjectCode);
        //}

        //// Update an existing subject
        //public static bool UpdateSubject(string subjectCode, Subject updatedSubject)
        //{
        //    var subjects = GetAllSubjects();
        //    var subjectToUpdate = subjects.FirstOrDefault(s => s.SubjectCode == subjectCode);
        //    if (subjectToUpdate != null)
        //    {
        //        subjectToUpdate.SubjectName = updatedSubject.SubjectName;
        //        subjectToUpdate.Description = updatedSubject.Description;
        //        subjectToUpdate.Teacher = updatedSubject.Teacher;
        //        subjectToUpdate.Status = updatedSubject.Status;

        //        // Re-write the CSV file with updated data
        //        using (var writer = new StreamWriter(filePath))
        //        {
        //            writer.WriteLine("SubjectCode,SubjectName,Description,Teacher,Status");
        //            foreach (var subject in subjects)
        //            {
        //                writer.WriteLine($"{subject.SubjectCode},{subject.SubjectName},{subject.Description},{subject.Teacher},{subject.Status}");
        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}


        //// Delete a subject by its SubjectCode
        //public static bool DeleteSubject(string subjectCode)
        //{
        //    var subjects = GetAllSubjects();
        //    var subjectToDelete = subjects.FirstOrDefault(s => s.SubjectCode == subjectCode);
        //    if (subjectToDelete != null)
        //    {
        //        subjects.Remove(subjectToDelete);
        //        // Re-write the CSV file without the deleted subject
        //        using (var writer = new StreamWriter(filePath))
        //        {
        //            writer.WriteLine("SubjectCode,SubjectName,Description,Teacher,Status");
        //            foreach (var subject in subjects)
        //            {
        //                writer.WriteLine($"{subject.SubjectCode},{subject.SubjectName},{subject.Description},{subject.Teacher},{subject.Status}");
        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}

    }
}
