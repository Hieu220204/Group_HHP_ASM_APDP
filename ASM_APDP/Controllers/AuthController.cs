using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;

public class AuthController : Controller
{
    // Login page
    public IActionResult Login()
    {
        return View();
    }

    // Handle login POST request
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Admin login check
        if (email == "admin@gmail.com" && password == "admin123")
        {
            return RedirectToAction("AdminHome", "Admin");
        }

        // Check student login
        var student = Student.GetStudentByEmail(email, password);  // This method now accepts both email and password
        if (student != null)
        {
            // Set session to store the user's email for future use
            HttpContext.Session.SetString("UserEmail", email);
            return RedirectToAction("StudentHome", "Student");
        }

        // Check teacher login
        var teacher = Teacher.GetTeacherByEmail(email, password);  // Now calling GetTeacherByEmail
        if (teacher != null)
        {
            return RedirectToAction("TeacherHome", "Teacher");
        }

        // If no match found for both student and teacher, show error message
        ViewBag.Error = "Invalid login credentials. Please try again.";
        return View();
    }

    // Logout action
    public IActionResult Logout()
    {
        // Clear session data to log out the user
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");  // Redirect to Login page
    }

    // Register page
    public IActionResult Register()
    {
        return View();
    }

    // Handle registration POST request


    [HttpPost]
    public IActionResult Register(string fullName, string email, string password, string dob, string phoneNumber, string hometown, string major)
    {
        if (Student.IsEmailExist(email))
        {
            ViewBag.ErrorMessage = "❌ This email has already been registered. Please choose another email.";
            return View();
        }

        // Lưu vào Student.csv
        Student.SaveStudent(fullName, email, password, dob, phoneNumber, hometown, major);

        ViewBag.SuccessMessage = "✅ Registration successful! You can log in now.";
        return View();
    }




}

