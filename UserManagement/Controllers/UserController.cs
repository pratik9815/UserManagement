using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUserById(string id)
        {
            return View();
        }

    }
}
