using DataLayer.DataContext;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.DTOs;
using UserManagement.Application.IServices;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Infrastructure.Repositories;
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IGlobalFunction _globalFunction;

    public AuthService(ApplicationDbContext context, 
        IJwtService jwtService, 
        IHttpContextAccessor httpContextAccessor, 
        IConfiguration configuration, 
        IGlobalFunction globalFunction)
    {
        _context = context;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _globalFunction = globalFunction;
    }
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var authRes = new AuthResponse();
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.UserClaims)
                .ThenInclude(uc => uc.Claim)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            authRes.Code = 400;
            authRes.Message = "Invalid username or password";
            return authRes;
        }

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

        var claims = new List<Claim>
        {
            new Claim("FavouriteColor", "Blue"),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(user.UserClaims.Select(uc =>
           new Claim(uc.Claim.ClaimType, uc.ClaimValue)).ToList());


        var token = _jwtService.GenerateToken(user, roles);

        //_httpContextAccessor.HttpContext.Response.Cookies.Append("Token", token, new CookieOptions
        //{
        //    HttpOnly = true,
        //    SameSite = SameSiteMode.Strict,
        //    Secure = true
        //});

        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Code = 0,
            Message = "Login successful",
            Token = token,
            Username = user.Username,
            Roles = roles,
            Expiration = DateTime.Now.AddHours(1)
        };
    }

    public async Task<AuthResponse> RegisterAsync(CreateApplicationUserDTO request)
    {
        var authRes = new AuthResponse();
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            authRes.Code = 400;
            authRes.Message = "Username already exists";
            return authRes;
        }

        CreatePasswordHash(request.Password, out string passwordHash, out string passwordSalt);

        var user = new ApplicationUser
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Assign default role
        var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (defaultRole != null)
        {
            _context.UserRoles.Add(new ApplicationUserRole { UserId = user.Id, RoleId = defaultRole.Id });
            await _context.SaveChangesAsync();
        }
        authRes.Code = 0;
        authRes.Message = "User created successfully";
        return authRes;
        //return await LoginAsync(new LoginRequest { Username = request.Username, Password = request.Password });
    }

    private bool VerifyPassword(string password, string passwordHash, string passwordSalt)
    {
        using (var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt)))
        {
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == passwordHash;
        }
    }

    private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }

    
    
    public string ValidateToken(string token)
    {
        ClaimsPrincipal principal = _globalFunction.GetPrinciple(token);
        if (principal == null)
            return null;
        ClaimsIdentity identity = null;
        try
        {
            identity = (ClaimsIdentity)principal.Identity;
        }
        catch (Exception ex)
        {
            return null;
        }
        Claim userClaim = identity.FindFirst(ClaimTypes.Name);
        string username = userClaim.Value;
        return username;
    }

   
}
