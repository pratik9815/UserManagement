using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Services;

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
}
