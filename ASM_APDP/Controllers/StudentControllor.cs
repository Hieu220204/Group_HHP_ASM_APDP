using Microsoft.AspNetCore.Mvc;

namespace ASM_APDP.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult StudentHome()
        {
            return View();
        }
    }
}
