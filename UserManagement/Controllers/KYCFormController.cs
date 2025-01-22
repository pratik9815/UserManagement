using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Services;
using UserManagement.Application.ViewModel;

namespace UserManagement.Api.Controllers;

public class KYCFormController : Controller
{
    private string SessionKey = "KYCForm";
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Step1()
    {
        var model = new PersonalDetails();
        return PartialView("_personalDetails", model);
    }
    [HttpPost]
    public IActionResult Step1(PersonalDetails model) 
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_personalDetails", model);
        }
        var sessionModel = HttpContext.Session.Get<UserFormViewModel>(SessionKey) ?? new UserFormViewModel();
        sessionModel.PersonalDetails = model;
        HttpContext.Session.Set(SessionKey, sessionModel);
        return RedirectToAction("familyDetails");
    }
    [HttpGet]
    public IActionResult Step2()
    {
        var model = new FamilyDetails();
        return PartialView("_familyDetails", model);
    }

    [HttpPost]
    public IActionResult Step2(FamilyDetails model)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_familyDetails", model);
        }

        var sessionModel = HttpContext.Session.Get<UserFormViewModel>(SessionKey);
        sessionModel.FamilyDetails = model;
        HttpContext.Session.Set(SessionKey, sessionModel);

        return RedirectToAction("requiredFiles");
    }
    [HttpGet]
    public IActionResult Step3()
    {
        var model = new RequiredFiles();
        return PartialView("_requiredFiles", model);
    }
    [HttpPost]
    public IActionResult Step3(RequiredFiles model)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_requiredFiles", model);
        }

        var sessionModel = HttpContext.Session.Get<UserFormViewModel>(SessionKey);
        sessionModel.RequiredFiles = model;

        // Process the complete form data
        // ...

        // Clear session after processing
        HttpContext.Session.Remove(SessionKey);

        return RedirectToAction("Success");
    }
    public IActionResult Success()
    {
        return View();
    }


}
