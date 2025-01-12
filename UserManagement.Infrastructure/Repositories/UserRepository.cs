using Dapper;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.DataContext;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    public readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
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
}
