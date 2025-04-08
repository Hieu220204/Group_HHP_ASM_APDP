using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services;

public class SubjectController : Controller
{
    private readonly SubjectService _subjectService;

    // Inject the SubjectService into the controller via constructor
    public SubjectController(SubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    // GET: /Admin/ManageSubject
    public IActionResult ManageSubject()
    {
        var subjects = _subjectService.GetAllSubjects();
        return View("~/Views/Admin/ManageSubject.cshtml", subjects);
    }

    // POST: /Admin/AddSubject
    [HttpPost]
    public IActionResult Add(Subject subject)
    {
        if (ModelState.IsValid)
        {
            _subjectService.AddSubject(subject);
            return RedirectToAction("ManageSubject");
        }
        return View(subject);
    }

    // GET: /Admin/EditSubject/subjectCode
    public IActionResult Edit(string subjectCode)
    {
        var subject = _subjectService.GetSubjectByCode(subjectCode);
        if (subject != null)
        {
            ViewBag.SelectedSubject = subject;
            return View("~/Views/Admin/ManageSubject.cshtml", _subjectService.GetAllSubjects());
        }
        return RedirectToAction("ManageSubject");
    }

    // POST: /Admin/EditSubject
    [HttpPost]
    public IActionResult Edit(string subjectCode, Subject updatedSubject)
    {
        if (ModelState.IsValid)
        {
            var success = _subjectService.UpdateSubject(subjectCode, updatedSubject);
            if (success)
            {
                return RedirectToAction("ManageSubject");
            }
            ViewBag.ErrorMessage = "Update failed. Please try again.";
        }
        return View(updatedSubject);
    }

    // POST: /Admin/DeleteSubject
    [HttpPost]
    public IActionResult Delete(string subjectCode)
    {
        var success = _subjectService.DeleteSubject(subjectCode);
        if (success)
        {
            return RedirectToAction("ManageSubject");
        }
        ViewBag.ErrorMessage = "Delete failed. Please try again.";
        return RedirectToAction("ManageSubject");
    }
}
