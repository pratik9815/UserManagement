using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Services;
using UserManagement.Application.DTOs;
using UserManagement.Application.IServices;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.IRepositories;
using UserManagement.Infrastructure.UOW;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<HomeController> _logger;
        private readonly IGlobalFunction _globalFunction;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwtService;
        public HomeController(ILogger<HomeController> logger, 
            IAuthService authService, 
            IGlobalFunction globalFunction, 
            IUserRepository userRepository,
            IUnitOfWork uow)
        {
            _logger = logger;
            _authService = authService;
            _globalFunction = globalFunction;
            _userRepository = userRepository;
            _uow = uow;
        }

        public IActionResult Index()
        {
            return View(new LoginRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }
                string LoginSessionId = Request.Cookies["session_id"]?.ToString();
                LoginCommon loginCommon = new()
                {
                    UserName = request.Username,
                    Password = request.Password,
                    IpAddress = _globalFunction.GetIp(),
                    BrowserInfo = _globalFunction.GetBrowser(),
                    SessionId = "",
                    IpInfo = "",
                    IpCountry = ""
                };

                UserCommon common = await _userRepository.UserLogin(loginCommon);

                if(common.Code != "0")
                {
                    TempData["Message"] = common.Msg;
                    return View(request);
                }
                var response = await _authService.LoginAsync(request);
                if (response.Code != 0)
                {
                    TempData["Message"] = response.Message;
                    return View(request);
                }
                HttpContext.Response.Cookies.Append("Token", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = true,
                    IsEssential = true,
                    //Expires
                });
                return SetSessionAfterLogin(request, common);
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
        //[ServiceFilter<SessionExpiryFilters>]
        public ActionResult LoginSuccess()
        {
            return RedirectToAction("Dashboard","Dashboard");
        }

        public ActionResult Logout()
        {
            Response.Cookies.Append("Token", "",new CookieOptions { Expires = DateTime.UtcNow.AddDays(-30)});
            Response.Cookies.Append("refreshtoken", "", new CookieOptions { Expires = DateTime.UtcNow.AddDays(-30) });

            HttpContext.Response.Cookies.Delete("Token");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
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
                if(response.Code == 0)
                {
                    TempData["Message"] = response.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = response.Message;
                    return View(request);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[ServiceFilter<SessionExpiryFilters>]
        //public ActionResult Dashboard()
        //{
        //    return View();
        //}

        public IActionResult SetSessionAfterLogin(LoginRequest request,UserCommon userCommon)
        {

            HttpContext.Session.SetString("MenuList", "Menu");
            var menuList = new List<string>();
            HttpContext.Session.SetString("MenuCode", JsonSerializer.Serialize(menuList));

            var token = HttpContext.Request.Cookies["Token"]?.ToString();

            //generate refresh token and insert it into authtoken table and set the cookie
            string refreshtoken = Guid.NewGuid().ToString();
            _uow.Login.InsertRefreshToken(refreshtoken, userCommon.Username);
            HttpContext.Response.Cookies.Append("refreshtoken", refreshtoken);
            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7)
            };
            HttpContext.Response.Cookies.Append("session_id","session", options);
            HttpContext.Session.SetString("logout_user_id", "logout_user_id");

            HttpContext.Session.SetString("logout_session_id", "logout_session_id");

            LoggedInUser loggedInUser = new LoggedInUser
            {
                UserId = userCommon.UserId,
                UserName = userCommon.Username,
                UserEmail = userCommon.Email,
                Browser = _globalFunction.GetBrowser(),
                IpAddress = _globalFunction.GetIp(),
                LoginTime = DateTime.UtcNow,
                LastLoginTime = DateTime.UtcNow,
                SessionId = HttpContext.Session.Id  
            };

            //var accesstoken = _jwtService.CheckRefreshToken(token);
            return RedirectToAction("Dashboard","Dashboard");
        }
    }
}
