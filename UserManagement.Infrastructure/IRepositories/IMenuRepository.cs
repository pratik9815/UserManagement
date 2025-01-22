using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.IRepositories;
public interface IMenuRepository
{
    List<MenuInfo> GetMenuList(); 
}
