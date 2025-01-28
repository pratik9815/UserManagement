using Dapper;
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
    public List<MenuInfo> GetMenuList(string username)
    {
        string sp = "LoadUser_Menu @username = @username";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@username", username);

        var res = _dapperContext.Query<MenuInfo>(sp, parameters).ToList();

        //string sql = "select *from main_menu order by menu_order asc";
        //var res = _dapperContext.Query<MenuInfo>(sql).ToList();
        return res;
    }
}
