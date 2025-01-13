using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs;
using UserManagement.Infrastructure.IRepositories;
using LoginRequest = UserManagement.Application.DTOs.LoginRequest;

namespace UserManagement.Api.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View(new LoginRequest());
        }
        [HttpPost]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Register()
        {
            return View(new CreateApplicationUserDTO());
        }   

        [HttpPost]
        public async Task<ActionResult<AuthResponse>> Register(CreateApplicationUserDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }
                var response = await _authService.RegisterAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
