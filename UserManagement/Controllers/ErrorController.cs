using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Api.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Unauthorized()
        {
            return View();
        }
        public IActionResult NotFound()
        {
            return View();
        }
        public IActionResult Server()
        {
            return View();
        }
    }
}
