using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASM_APDP.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeacherController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult TeacherHome()
        {
            var username = _httpContextAccessor.HttpContext?.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
