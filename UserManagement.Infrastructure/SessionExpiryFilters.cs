using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using UserManagement.Application.IServices;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.IRepositories;
using UserManagement.Infrastructure.UOW;

namespace UserManagement.Api.Services;
public class SessionExpiryFilters : ActionFilterAttribute
{
    private readonly IGlobalFunction _globalFunction;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _uow;
    public SessionExpiryFilters(IGlobalFunction globalFunction, IJwtService jwtService, IUnitOfWork uow)
    {
        _globalFunction = globalFunction;
        _jwtService = jwtService;
        _uow = uow;
    }
    public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
    {
        HttpContext httpContext = actionExecutingContext.HttpContext;
        //string queryString = context.HttpContext.Request.Headers["Authorization"].ToString();
        //HttpRequest? request = context.HttpContext.Request;
        //Uri realURL = new Uri(request.Scheme + "://" + request.Host.Value + request.PathBase + request.Path + request.QueryString);

        //Dictionary<string, StringValues>? parseQuery = QueryHelpers.ParseNullableQuery(realURL.Query);


        string returnUrl = httpContext.Request.Query["ReturnUrl"].ToString();

        //Authentication check
        string username = _globalFunction.GetClaims("username");
        //check for refresh token as well
        string refreshToken = httpContext.Request.Cookies["refreshtoken"]?.ToString();
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(refreshToken))
        {
            RedirectToLogOff(actionExecutingContext);
            //return;
        }

        //Set language

        //Initialize session data
        if (httpContext.Session.GetString("MenuList") == null)
        {
            List<MenuInfo> menuInfos = _uow.Menu.GetMenuList(username);
            httpContext.Session.SetString("MenuList", JsonConvert.SerializeObject(menuInfos));
        }
        if (httpContext.Session.GetString("MenuCode") == null)
        {
            List<string> listmenuCode = new List<string>();
            listmenuCode.Add("pratik");
            httpContext.Session.SetString("MenuCode", JsonConvert.SerializeObject(listmenuCode));
        }
        //Authorization check

        string token = httpContext.Request.Cookies["Token"]?.ToString();
        var accessToken = _jwtService.CheckRefreshToken(token);

        if (accessToken == null)
        {
            RedirectToLogOff(actionExecutingContext);
            return;
        }



        base.OnActionExecuting(actionExecutingContext);
        // If the browser session or authentication session has expired...


        //base.OnActionExecuting(context);
    }
    private void RedirectToLogOff(ActionExecutingContext context)
    {
        context.Result = new RedirectToRouteResult(new RouteValueDictionary
        {
            {"Controller", "Home"},
            { "Action", "Index" }
        });
    }
    private void RedirectToDashboard(ActionExecutingContext context)
    {
        context.Result = new RedirectToRouteResult(new RouteValueDictionary
        {
            {"Controller", "Dashboard" },
            {"Action", "Dashboard" }
        });
    }
}
