using Microsoft.AspNetCore.Mvc;

public class StudentController : Controller
{
    public IActionResult StudentHome()
    {
        return View();
    }
}
