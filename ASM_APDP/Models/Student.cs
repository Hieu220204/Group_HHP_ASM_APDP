using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASM_APDP.Models
{
    public class Student
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Hometown { get; set; }
        public string Major { get; set; }

        // Method to check if email already exists in the CSV file
        public static bool IsEmailExist(string email)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (!File.Exists(filePath))
            {
                return false; // If the file does not exist, return false
            }

            using (var reader = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; // Skip header line
                    }

                    var data = line.Split(',');

                    if (data.Length == 7 && data[1] == email) // Check if email exists
                    {
                        return true; // Email exists in the file
                    }
                }
            }

            return false; // If no match, return false
        }

        // Method to save student information to CSV
        public static void SaveStudent(string fullName, string email, string password, string dob, string phoneNumber, string hometown, string major)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Open the CSV file in append mode and write the new student info
            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{fullName},{email},{password},{dob},{phoneNumber},{hometown},{major}");
            }
        }

        // Method to get student profile by email
        public static Student GetStudentProfile(string email)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (!File.Exists(filePath))
            {
                return null; // If file does not exist, return null
            }

            using (var reader = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; // Skip header line
                    }

                    var data = line.Split(',');

                    // If the email matches, return the student profile
                    if (data.Length == 7 && data[1] == email)
                    {
                        return new Student
                        {
                            FullName = data[0],
                            Email = data[1],
                            Password = data[2],
                            DateOfBirth = data[3],
                            PhoneNumber = data[4],
                            Hometown = data[5],
                            Major = data[6]
                        };
                    }
                }
            }

            return null; // If no match, return null
        }

        // Method to update student profile information
        public static bool UpdateStudentInfo(string email, string fullName, string dob, string phoneNumber, string hometown, string major)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");
            var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student_temp.csv");

            if (!File.Exists(filePath))
            {
                return false; // If file does not exist, return false
            }

            bool isUpdated = false;

            using (var reader = new StreamReader(filePath))
            using (var writer = new StreamWriter(tempFilePath))
            {
                string line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        writer.WriteLine(line); // Write header line
                        isFirstLine = false;
                        continue;
                    }

                    var data = line.Split(',');

                    if (data.Length == 7 && data[1] == email) // If email matches, update the student's info
                    {
                        data[0] = fullName;
                        data[3] = dob;
                        data[4] = phoneNumber;
                        data[5] = hometown;
                        data[6] = major;
                        isUpdated = true;
                    }

                    writer.WriteLine(string.Join(",", data)); // Write updated or unchanged data
                }
            }

            // If update is successful, replace the original file with the new file
            if (isUpdated)
            {
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
            }

            return isUpdated;
        }

        // Method to get student by email and old password (for password change)
        public static Student GetStudentByEmail(string email, string oldPassword)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (!File.Exists(filePath))
            {
                return null; // If file does not exist, return null
            }

            using (var reader = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; // Skip header line
                    }

                    var data = line.Split(',');

                    // If the email and old password match, return the student object
                    if (data.Length == 7 && data[1] == email && data[2] == oldPassword)
                    {
                        return new Student
                        {
                            FullName = data[0],
                            Email = data[1],
                            Password = data[2],
                            DateOfBirth = data[3],
                            PhoneNumber = data[4],
                            Hometown = data[5],
                            Major = data[6]
                        };
                    }
                }
            }

            return null; // If no match, return null
        }

        // Method to update password
        public static bool UpdatePassword(string email, string newPassword)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");
            var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student_temp.csv");

            if (!File.Exists(filePath))
            {
                return false; // If file does not exist, return false
            }

            bool isUpdated = false;

            using (var reader = new StreamReader(filePath))
            using (var writer = new StreamWriter(tempFilePath))
            {
                string line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        writer.WriteLine(line); // Write header line
                        isFirstLine = false;
                        continue;
                    }

                    var data = line.Split(',');

                    if (data.Length == 7 && data[1] == email) // If email matches, update the password
                    {
                        data[2] = newPassword;
                        isUpdated = true;
                    }

                    writer.WriteLine(string.Join(",", data)); // Write updated or unchanged data
                }
            }

            // If update is successful, replace the original file with the new file
            if (isUpdated)
            {
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
            }

            return isUpdated;
        }
    }
}
