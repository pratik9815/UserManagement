using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Services;
using UserManagement.Domain.Entities;

namespace UserManagement.Api.Controllers;
public class DashboardController : Controller
{
    //[ValidateAntiForgeryToken]
    [ServiceFilter<SessionExpiryFilters>]
    public IActionResult Index()
    {
        return View();
    }
    [ServiceFilter<SessionExpiryFilters>]
    public IActionResult Dashboard()
    {
        return View();
    }
    [ServiceFilter<SessionExpiryFilters>]
    public IActionResult UserForm()
    {
        return View(new UserFormModel());
    }
    public IActionResult Step(int step = 1)
    {
        var model = TempData["FormData"] as UserFormModel ?? new UserFormModel();
        TempData.Keep("FormData"); // This keeps the data for the next request
        ViewBag.Step = step;
        return PartialView($"_step{step}", model); // Return the partial view for the specified step
    }
    [HttpPost]
    public IActionResult SaveStep(UserFormModel model, int currentStep)
    {
        //if (!ModelState.IsValid)
        //{
        //    return Json(new
        //    {
        //        success = false,
        //        errors = ModelState.Values
        //        .SelectMany(v => v.Errors)
        //        .Select(e => e.ErrorMessage)
        //    });
        //}

        // Save the entire model to TempData
        TempData["FormData"] = model;

        return Json(new { success = true });
    }
    public IActionResult SubmitForm()
    {
        return Content("Form submission success");
    }

}
