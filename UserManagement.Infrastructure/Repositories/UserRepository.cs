using Dapper;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.DataContext;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    public readonly DapperContext _context;
    private readonly GlobalFunction _globalFunction;

    public UserRepository(DapperContext context)
    {
        _context = context;
        //_globalFunction = globalFunction;
    }
    public async Task<UserCommon> UserLogin(LoginCommon loginCommon)    
    {
        string sql = "spa_user_cred";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@flag", 's');
        parameters.Add("@username", loginCommon.UserName);
        //parameters.Add("",loginCommon.Password);

        var objUserCommon = await _context.QueryStoredProcFirstOrDefaultAsync<UserCommon>(sql, parameters);
        return objUserCommon;
    }

    public List<RoleResponse> GetAllRoleList()
    {
        var sql = "select *from roles";
        List<RoleResponse> data = _context.Query<RoleResponse>(sql).ToList();
        return data;
    }

    public DbResponse AssignRole(string roleId, string userName)
    {
        string sql = "spa_application_role_agent_user";
        DynamicParameters para = new DynamicParameters();
        para.Add("@role_id",roleId);
        para.Add("@userId", userName);
        var res = _context.ExecuteScalar<DbResponse>(sql, para);
        return res;
    }
}
