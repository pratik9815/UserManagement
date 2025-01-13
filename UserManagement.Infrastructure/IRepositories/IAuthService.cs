using System.Security.Claims;
using UserManagement.Application.DTOs;

namespace UserManagement.Infrastructure.IRepositories;
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(CreateApplicationUserDTO request);
    string ValidateToken(string token);
    
}
