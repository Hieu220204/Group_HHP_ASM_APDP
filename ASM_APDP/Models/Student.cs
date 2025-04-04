using System;
using System.IO;

namespace ASM_APDP.Models
{
    public class Student
    {
        // Using the 'required' modifier to ensure these properties are set at initialization
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        // Save student information to Student.csv
        public static void SaveStudent(string fullName, string email, string password)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            // Check if the file exists, if not, create a new one
            if (!File.Exists(filePath))
            {
                // If the file does not exist, create a new file with a header
                File.WriteAllText(filePath, "FullName,Email,Password\n");
            }

            // Add data to the CSV file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{fullName},{email},{password}");
            }
        }

        // Check if the email already exists
        public static bool IsEmailExist(string email)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                return false; // If the file does not exist, return false (email does not exist)
            }

            // Check if the email exists in the file
            foreach (var line in File.ReadLines(filePath))
            {
                var data = line.Split(',');

                // If the array length is 3 and the email matches, return true
                if (data.Length == 3 && data[1].Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return true; // Email exists
                }
            }

            return false; // If not found, email does not exist
        }

        // Get student information by email and password
        public static Student GetStudentByEmail(string email, string password)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    var data = line.Split(',');

                    // Check if the email and password match
                    if (data.Length == 3 && data[1] == email && data[2] == password)
                    {
                        return new Student { FullName = data[0], Email = data[1], Password = data[2] };
                    }
                }
            }

            return null; // If no student is found with the email and password
        }

        // Update the student's password
        public static bool UpdatePassword(string email, string newPassword)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student.csv");
            var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Student_temp.csv");

            if (!File.Exists(filePath))
            {
                return false; // If the file does not exist, return false
            }

            bool isUpdated = false;

            using (var reader = new StreamReader(filePath))
            using (var writer = new StreamWriter(tempFilePath))
            {
                // Read each line from the CSV file and copy it to the temporary file
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(',');

                    if (data.Length == 3 && data[1] == email) // If email is found, update the password
                    {
                        // Update the password
                        data[2] = newPassword;
                        isUpdated = true;
                    }

                    writer.WriteLine(string.Join(",", data));
                }
            }

            // After updating, replace the old file with the temporary file
            if (isUpdated)
            {
                File.Delete(filePath); // Delete the old file
                File.Move(tempFilePath, filePath); // Rename the temporary file to the old file
            }

            return isUpdated; // Return true if the update is successful
        }
    }
}
