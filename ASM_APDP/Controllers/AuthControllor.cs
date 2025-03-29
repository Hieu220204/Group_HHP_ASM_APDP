using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services;

namespace ASM_APDP.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthManagement _authManager;

        public AuthController(AuthManagement authManager)
        {
            _authManager = authManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string role = _authManager.Login(model.Username, model.Password);
                if (role != null)
                {
                    return role switch
                    {
                        "Admin" => RedirectToAction("AdminHome", "Admin"),
                        _ => RedirectToAction("StudentHome", "Student")
                    };
                }
                ViewData["ErrorMessage"] = "Invalid username or password.";
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            _authManager.Logout();
            return RedirectToAction("Login");
        }
    }
}
