using Microsoft.AspNetCore.Mvc;

namespace ASM_APDP.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminHome()
        {
            if (HttpContext.Session.GetString("role") != "Admin")
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
