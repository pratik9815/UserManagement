using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Infrastructure.Repositories;
public class GlobalFunction : IGlobalFunction
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IDataProtectionProvider _protectionProvider;

    public GlobalFunction(IHttpContextAccessor httpContextAccessor, 
                        IConfiguration configuration, 
                        IDataProtectionProvider protectionProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _protectionProvider = protectionProvider;
    }

    public string GetBrowser()
    {
        string browser = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
        return browser;
    }

    public string GetIp()
    {
        string ip = _httpContextAccessor.HttpContext.Connection.LocalIpAddress?.ToString();
        if (string.IsNullOrEmpty(ip))
        {
            ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
        }
        return ip;
    }
    public string Encrypt(string clearText)
    {
        if(string.IsNullOrEmpty(clearText))
        {
            return "";
        }
        var plainTextBytes = Encoding.UTF8.GetBytes(clearText);
        var key = _configuration["Jwt:Key"];

        IDataProtector protector = _protectionProvider.CreateProtector(key);
        var encryptedData = Convert.ToBase64String(protector.Protect(plainTextBytes));
        encryptedData = WebUtility.UrlEncode(encryptedData);
        return encryptedData;
    }
    public string Decrypt(string cipherText)
    {
        try
        {
            if(string.IsNullOrEmpty(cipherText))
            {
                return "";
            }
            cipherText = WebUtility.UrlDecode(cipherText);
            string? key = _configuration["Jwt:Key"];

            cipherText = cipherText.Replace(" ", "+");
            IDataProtector protector = _protectionProvider.CreateProtector(key);
            var bytes = Convert.FromBase64String(cipherText);
            var output = protector.Unprotect(bytes);
            cipherText = Encoding.UTF8.GetString(output);
            return cipherText;
        }
        catch
        {
            throw;
        }
    }
    public ClaimsPrincipal GetPrinciple(string token)
    {
        try
        {
            string _jwtSecretKey = _configuration["Jwt:Key"];
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            if (jwtToken == null)
                return null;

            //byte[] key = Convert.FromBase64String(_jwtSecretKey);
            byte[] key = Encoding.UTF8.GetBytes(_jwtSecretKey);
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                RequireExpirationTime = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }
    public string GetUser()
    {
        var user = GetClaims("username");
        return user;
    }
    public string GetClaims(string key)
    {
        try
        {
            HttpContext? currentHttpContext = _httpContextAccessor.HttpContext;

            string token = currentHttpContext.Request.Cookies["Token"]?.ToString();
            if (currentHttpContext == null)
                return null;
            if (string.IsNullOrEmpty(token))
                return null;

            ClaimsPrincipal objClaimPrinciple = GetPrinciple(token);
            if (objClaimPrinciple == null)
                return null;
            ClaimsIdentity claimsIdentity = null;
            try
            {
                claimsIdentity = (ClaimsIdentity)objClaimPrinciple.Identity;
            }
            catch (Exception ex)
            {
                return null;
            }
            Claim claims = claimsIdentity.FindFirst(key);
            return Decrypt(claims?.Value);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
