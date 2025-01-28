using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.IRepositories;

public interface IUserRepository
{
    Task<UserCommon> UserLogin(LoginCommon loginCommon);
    List<UserResponse> GetUserList();
    UserResponse GetUserById(int? id);
    List<RoleResponse> GetAllRoles();
    DbResponse UpdateUser(UserCommon user);

}
