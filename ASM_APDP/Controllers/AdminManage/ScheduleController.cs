using Microsoft.AspNetCore.Mvc;
using ASM_APDP.Models;
using ASM_APDP.Services;

public class ScheduleController : Controller
{
    private readonly ScheduleService _scheduleService;

    // Constructor to inject the ScheduleService
    public ScheduleController(ScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    private IActionResult LoadSchedulesWithError(string errorMessage = "")
    {
        var schedules = _scheduleService.GetAllSchedules();
        ViewBag.ErrorMessage = errorMessage; // Pass error message to the view
        return View("~/Views/Admin/ManageSchedule.cshtml", schedules);
    }

    public IActionResult ManageSchedule()
    {
        // Get all schedules using ScheduleService
        return LoadSchedulesWithError();
    }

    [HttpPost]
    public IActionResult Add(Schedule schedule)
    {
        // Check if model is valid before adding
        if (ModelState.IsValid)
        {
            _scheduleService.AddSchedule(schedule); // Use service to add schedule
            return RedirectToAction("ManageSchedule");
        }

        // Return the same view with all schedules in case of validation error
        return LoadSchedulesWithError("❌ Failed to add the schedule. Please check the input.");
    }

    public IActionResult Edit(string className)
    {
        // Fetch the schedule by class name using ScheduleService
        var schedule = _scheduleService.GetScheduleByClassName(className);
        if (schedule != null)
        {
            ViewBag.SelectedSchedule = schedule;
        }
        // Return all schedules to the view
        return LoadSchedulesWithError();
    }

    [HttpPost]
    public IActionResult Edit(string className, Schedule updatedSchedule)
    {
        // Check if model is valid before updating
        if (ModelState.IsValid)
        {
            // Use service to update schedule
            var success = _scheduleService.UpdateSchedule(className, updatedSchedule);
            if (success)
            {
                return RedirectToAction("ManageSchedule");
            }

            // If update fails, show an error message
            return LoadSchedulesWithError("❌ Failed to update the schedule.");
        }

        // Return all schedules if the model state is invalid
        return LoadSchedulesWithError("❌ Failed to update the schedule. Please check the input.");
    }

    [HttpPost]
    public IActionResult Delete(string className)
    {
        // Use service to delete the schedule
        _scheduleService.DeleteSchedule(className);
        return RedirectToAction("ManageSchedule");
    }

    // Action to view class schedule
    public IActionResult ViewClassSchedule()
    {
        // Fetch the teacher's class schedule (you might need to get the teacher's specific classes)
        var schedules = _scheduleService.GetAllSchedules(); // Assuming this returns all schedules

        // Return the schedules to the view
        return View("~/Views/Teacher/ViewClassSchedule.cshtml", schedules);
    }
}
