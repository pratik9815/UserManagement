using DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Application.IServices;
using UserManagement.Infrastructure.IRepositories;
using UserManagement.Infrastructure.UOW;

namespace UserManagement.Application.Services;
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IGlobalFunction _globalFunction;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _uow;


    public JwtService(IConfiguration configuration, IGlobalFunction globalFunction, IHttpContextAccessor httpContextAccessor, IUnitOfWork uow)
    {
        _configuration = configuration;
        _globalFunction = globalFunction;
        _httpContextAccessor = httpContextAccessor;
        _uow = uow;
    }
    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor descriptor = new();
        descriptor.SigningCredentials = credentials;
        //var claimsList = new List<Claim>
        //{
        //    new Claim(ClaimTypes.NameIdentifier, _globalFunction.Encrypt(user.Id.ToString())),
        //    new Claim(ClaimTypes.Name, _globalFunction.Encrypt(user.Username)),
        //    new Claim(ClaimTypes.Email, _globalFunction.Encrypt(user.Email)),
        //    new Claim("Email", _globalFunction.Encrypt(user.Email)),
        //    new Claim("username", _globalFunction.Encrypt("pratik"))
        //};
        descriptor.Subject = new ClaimsIdentity();
        descriptor.Subject.AddClaim(new Claim(ClaimTypes.NameIdentifier, _globalFunction.Encrypt(user.Id.ToString())));
        descriptor.Subject.AddClaim(new Claim(ClaimTypes.Name, _globalFunction.Encrypt(user.Username)));
        descriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, _globalFunction.Encrypt(user.Email)));
        descriptor.Subject.AddClaim(new Claim("Email", _globalFunction.Encrypt(user.Email)));
        descriptor.Subject.AddClaim(new Claim("username", _globalFunction.Encrypt(user.Username)));
        //descriptor.Subject.AddClaim(new Claim("UserLoginId", _globalFunction.Encrypt(user.Username)));
        // Add roles
        foreach (var role in roles)
        {
            descriptor.Subject.AddClaim(new Claim("UserRole", _globalFunction.Encrypt(role)));
        }

        // Add custom claims
        //claimsList.AddRange(claims);
        //JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        //JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: descriptor.Subject.Claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CheckRefreshToken(string access_token)
    {
        bool cookieAdded = _httpContextAccessor.HttpContext.Response.Headers.ContainsKey("Set-Cookie");
        if (cookieAdded)
        {
            return access_token;
        }
        ClaimsPrincipal principals = _globalFunction.GetPrinciple(access_token);
        if (principals == null)
            return null;
        var expiryDateUnix = long.Parse(principals.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateUtc = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if(expiryDateUtc < DateTime.UtcNow)
        {
            string refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshtoken"].ToString();
            if (refreshToken == null)
                return null;
            string userLoginId = principals.Claims.Single(x => x.Type == "username")?.Value.ToString();

            var username = _globalFunction.Decrypt(userLoginId);
            //check if refreshTokenIsValid and was assigned for this user 
            int authTokenId = _uow.Login.CheckIfRefreshTokenIsValid(refreshToken, username);
            if (authTokenId <= 0) return null;
            // this refresh token is valid;so revoke this refresh token and provide new refresh token 
            _uow.Login.RevokeThisRefreshToken(authTokenId.ToString());

            //need to generate new access token
            List<string> role = principals.Claims
                            .Where(x => x.Type == "UserRole")
                            .Select(x => _globalFunction.Decrypt(x.Value))
                            .ToList();
            string email = _globalFunction.Decrypt(principals.Claims.Single(x => x.Type == "Email").Value.ToString());
            var accesstoken = GenerateToken(new ApplicationUser { Username = username, Email = email }, role);
            var httpResponse = _httpContextAccessor.HttpContext.Response;
            httpResponse.Cookies.Append("Token", accesstoken);
            string newRefreshToken = Guid.NewGuid().ToString();
            _uow.Login.InsertRefreshToken(newRefreshToken, username);
            httpResponse.Cookies.Append("refreshtoken", newRefreshToken);
            return accesstoken;
        }
        return access_token;
    }
}
