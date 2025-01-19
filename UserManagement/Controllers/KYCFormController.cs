using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Api.Controllers;

public class KYCFormController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
