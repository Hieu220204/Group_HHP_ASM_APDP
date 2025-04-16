using ASM_APDP.Models;

namespace ASM_APDP.Controllers.StudentManage
{
    public class StudentManagement
    {
        public static string GetFilePath()
        {
            return filePath;
        }
        public static List<Student> Students { get; set; } = new List<Student>();
        private static StudentManagement _instance;
        private static readonly object _lock = new object();
        internal static readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Students.csv");
        private static readonly string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Students_temp.csv");

        private StudentManagement() { }

        public static StudentManagement GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new StudentManagement();
                }
                return _instance;
            }
        }

        // Get all students from CSV
        public static List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines.Skip(1))
                {
                    var values = line.Split(',');
                    if (values.Length == 7)
                    {
                        students.Add(new Student
                        {
                            FullName = values[0],
                            Email = values[1],
                            Password = values[2],
                            DateOfBirth = values[3],
                            PhoneNumber = values[4],
                            Hometown = values[5],
                            Major = values[6]
                        });
                    }
                }
            }

            return students;
        }

        // Save the list of students to the CSV file
        public static void SaveStudents(List<Student> students)
        {
            var lines = new List<string> { "FullName,Email,Password,DateOfBirth,PhoneNumber,Hometown,Major" };

            foreach (var student in students)
            {
                lines.Add($"{student.FullName},{student.Email},{student.Password},{student.DateOfBirth},{student.PhoneNumber},{student.Hometown},{student.Major}");
            }

            File.WriteAllLines(filePath, lines);
        }

        // Check if the email already exists in the student list
        public static bool IsEmailExist(string email)
        {
            var students = GetAllStudents();
            return students.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // Get student by email and old password (used for password change)
        public static Student GetStudentByEmail(string email, string oldPassword)
        {
            if (Students != null && Students.Any())
            {
                return Students.FirstOrDefault(s => s.Email == email && s.Password == oldPassword);
            }

            if (!File.Exists(filePath))
            {
                return null;
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
                        continue;
                    }

                    var data = line.Split(',');

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

            return null;
        }

        // Get student profile by email
        public static Student GetStudentProfile(string email)
        {
            if (!File.Exists(filePath))
            {
                return null;
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
                        continue;
                    }

                    var data = line.Split(',');

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

            return null;
        }

        // Update student profile information
        public static bool UpdateStudentInfo(string email, string fullName, string dob, string phoneNumber, string hometown, string major)
        {
            if (!File.Exists(filePath))
            {
                return false;
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
                        writer.WriteLine(line);
                        isFirstLine = false;
                        continue;
                    }

                    var data = line.Split(',');

                    if (data.Length == 7 && data[1] == email)
                    {
                        data[0] = fullName;
                        data[3] = dob;
                        data[4] = phoneNumber;
                        data[5] = hometown;
                        data[6] = major;
                        isUpdated = true;
                    }

                    writer.WriteLine(string.Join(",", data));
                }
            }

            if (isUpdated)
            {
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
            }

            return isUpdated;
        }

        // Update student password
        public static bool UpdatePassword(string email, string newPassword)
        {
            if (!File.Exists(filePath))
            {
                return false;
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
                        writer.WriteLine(line);
                        isFirstLine = false;
                        continue;
                    }

                    var data = line.Split(',');

                    if (data.Length == 7 && data[1] == email)
                    {
                        data[2] = newPassword;
                        isUpdated = true;
                    }

                    writer.WriteLine(string.Join(",", data));
                }
            }

            if (isUpdated)
            {
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
                Students = GetAllStudents();
            }

            return isUpdated;
        }
    }
}
