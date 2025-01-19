using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using UserManagement.Application.IServices;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Api.Services;
public class SessionExpiryFilters : ActionFilterAttribute
{
    private readonly IGlobalFunction _globalFunction;
    private readonly IJwtService _jwtService;
    public SessionExpiryFilters(IGlobalFunction globalFunction, IJwtService jwtService)
    {
        _globalFunction = globalFunction;
        _jwtService = jwtService;
    }
    public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
    {
        HttpContext httpContext = actionExecutingContext.HttpContext;
        //string queryString = context.HttpContext.Request.Headers["Authorization"].ToString();
        //HttpRequest? request = context.HttpContext.Request;
        //Uri realURL = new Uri(request.Scheme + "://" + request.Host.Value + request.PathBase + request.Path + request.QueryString);

        //Dictionary<string, StringValues>? parseQuery = QueryHelpers.ParseNullableQuery(realURL.Query);

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

        //Authorization check

        string token = httpContext.Request.Cookies["Token"]?.ToString();
        var accessToken = _jwtService.CheckRefreshToken(token);

        if (accessToken == null)
        {
            RedirectToLogOff(actionExecutingContext);
            return;
        }
        //RedirectToDashboard(actionExecutingContext);
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
