using DataLayer.Entities;

namespace UserManagement.Application.IServices;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser> GetUserByIdAsync(string id);
    //Task CreateUserAsync(CreateUserDto userDto);
    Task DeleteUserAsync(string id);
}
