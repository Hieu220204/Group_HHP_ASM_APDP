using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ASM_APDP.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult AdminHome()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null || session.GetString("role") != "Admin")
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
