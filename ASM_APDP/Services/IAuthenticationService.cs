using ASM_APDP.Controllers.StudentManage;
using ASM_APDP.Models;

public interface IAuthenticationService
{
    object Authenticate(string email, string password);
}

public class AuthenticationService : IAuthenticationService
{
    public object Authenticate(string email, string password)
    {
        // Check for admin credentials
        if (email == "admin@gmail.com" && password == "admin123")
        {
            return new AdminController(); // Or a suitable Admin object
        }

        // Try finding student by email and password
        var student = StudentManagement.GetStudentByEmail(email, password);
        if (student != null)
        {
            return student; // Return student object
        }

        // Try finding teacher by email and password
        var teacher = Teacher.GetTeacherByEmail(email, password);
        if (teacher != null)
        {
            return teacher; // Return teacher object
        }

        return null; // Return null if no user is found
    }

    private readonly List<Student> _students = new List<Student>
    {
        new Student { Email = "student@gmail.com", Password = "student123" }
    };
}
