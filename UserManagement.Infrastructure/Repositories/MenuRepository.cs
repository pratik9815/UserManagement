using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.DataContext;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Infrastructure.Repositories;
public class MenuRepository : IMenuRepository
{
    private readonly DapperContext _dapperContext;
    public MenuRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    public List<MenuInfo> GetMenuList()
    {
        string sql = "select *from main_menu";
        var res = _dapperContext.Query<MenuInfo>(sql).ToList();
        return res;
    }
}
