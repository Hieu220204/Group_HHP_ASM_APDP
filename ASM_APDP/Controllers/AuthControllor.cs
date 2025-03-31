using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services;
using Microsoft.AspNetCore.Http;

namespace ASM_APDP.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthManagement _authManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(AuthManagement authManager, IHttpContextAccessor httpContextAccessor)
        {
            _authManager = authManager;
            _httpContextAccessor = httpContextAccessor;
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
                    var session = _httpContextAccessor.HttpContext?.Session;
                    if (session != null)
                    {
                        session.SetString("username", model.Username);
                        session.SetString("role", role);
                    }

                    return role switch
                    {
                        "Admin" => RedirectToAction("AdminHome", "Admin"),
                        "Teacher" => RedirectToAction("TeacherHome", "Teacher"),
                        _ => RedirectToAction("StudentHome", "Student")
                    };
                }
                ViewData["ErrorMessage"] = "Invalid username or password.";
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
