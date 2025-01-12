using DataLayer.Entities;
using Microsoft.AspNet.Identity;

namespace UserManagement.Application.IServices;
public interface IJwtService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
    string CheckRefreshToken(string access_token);
}
