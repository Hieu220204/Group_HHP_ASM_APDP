using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using System;

public class RegisterController : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string fullName, string email, string password)
    {
        try
        {
            if (Student.IsEmailExist(email))
            {
                ViewBag.ErrorMessage = "This email has already been registered. Please choose a different email.";
                return View();
            }

            Student.SaveStudent(fullName, email, password);

            // 🛠 Check if the data has been saved
            if (!Student.IsEmailExist(email))
            {
                ViewBag.ErrorMessage = "An error occurred, the account could not be saved!";
                return View();
            }

            TempData["SuccessMessage"] = "Registration successful! You can log in now.";
            return View("Register"); // Do not automatically redirect to Login
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "An error occurred while registering the account: " + ex.Message;
            return View();
        }
    }
}
