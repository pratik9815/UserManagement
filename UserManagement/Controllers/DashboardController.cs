using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserManagement.Api.Services;
using UserManagement.Domain.Entities;

namespace UserManagement.Api.Controllers;
public class DashboardController : Controller
{
    private const string SessionKey = "UserForm";
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
        if (HttpContext.Session.GetString(SessionKey) == null)
        {
            var newModel = new UserFormModel();
            HttpContext.Session.SetString(SessionKey, JsonConvert.SerializeObject(newModel));
        }
        return View();
    }
    public IActionResult Step(int step = 1)
    {
        var model = GetModelFromTempData();

        TempData.Keep(SessionKey);
        ViewBag.Step = step;
        return PartialView($"_step{step}", model); // Return the partial view for the specified step
    }
    [HttpPost]
    public IActionResult SaveStep(UserFormModel model, int currentStep)
    {
        try
        {
            var existingModel  = GetModelFromTempData();
            UpdateModelPropTempData(existingModel, model);
            SaveToTempData(existingModel);
            TempData.Keep(SessionKey);
            return PartialView($"_step{currentStep}", model);
        }
        catch (Exception ex)
        {
            return PartialView($"_step{currentStep - 1}", model);
        }
    }
    public IActionResult FinalResult(UserFormModel model, int currentStep)
    {
        var existingModel = GetModelFromTempData();
        UpdateModelPropTempData(existingModel, model);
        SaveToTempData(existingModel);
        TempData.Keep(SessionKey);
        return PartialView("_finalResult",model);
    }
    public IActionResult FinalStep(int step)
    {
        ViewBag.Step = step;
        var model = GetModelFromTempData();
        return PartialView("_finalResult", model);
    }

    [ServiceFilter<SessionExpiryFilters>]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitForm()
    {
        var model = GetModelFromTempData();
        TempData.Remove(SessionKey);
        return PartialView("_formSubmitted");
    }
    // Helper Methods
    private void SaveModelToSession(UserFormModel model)
    {
        var serializedModel = JsonConvert.SerializeObject(model);
        HttpContext.Session.SetString(SessionKey, serializedModel);
    }
    private UserFormModel GetModelFromSession()
    {
        var serializedModel = HttpContext.Session.GetString(SessionKey);
        return serializedModel != null ? JsonConvert.DeserializeObject<UserFormModel>(serializedModel) : new UserFormModel();
    }
    private void UpdateModelProperties(UserFormModel existing, UserFormModel incoming)
    {
        foreach (var property in typeof(UserFormModel).GetProperties())
        {
            var value = property.GetValue(incoming);
            if (value != null)
            {
                property.SetValue(existing, value);
            }
        }
    }
    private void SaveToTempData(UserFormModel model)
    {
        string obj = JsonConvert.SerializeObject(model);
        TempData[SessionKey] = obj;
    }
    private UserFormModel GetModelFromTempData()
    {
        if (!TempData.ContainsKey(SessionKey))
            return new UserFormModel();

        var json = TempData[SessionKey]?.ToString();
        //TempData.Keep(SessionKey);  // Keep the data for next request
        return string.IsNullOrEmpty(json)
            ? new UserFormModel()
            : JsonConvert.DeserializeObject<UserFormModel>(json);
    }
    private void UpdateModelPropTempData(UserFormModel existing, UserFormModel incomming)
    {
        foreach(var property in typeof(UserFormModel).GetProperties())
        {
            var value = property.GetValue(incomming);
            if (value!=null)
            {
                property.SetValue(existing, value);
            }
        }
    }
}
