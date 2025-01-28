using Dapper;
using UserManagement.Application.DTOs;
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

    public List<RoleResponse> GetAllRoles()
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

    public List<UserResponse> GetUserList()
    {
        string sp = "spa_user_list @flag = @flag";
        DynamicParameters para = new DynamicParameters();
        para.Add("@flag", 's');
        var res = _context.Query<UserResponse>(sp,para).ToList();
        return res;
    }

    public UserResponse GetUserById(int? id)
    {
        string sp = "spa_user_list @flag = @flag,@user_id = @userid";
        DynamicParameters para = new DynamicParameters();
        para.Add("@flag", 's');
        para.Add("@userid", id);
        var res = _context.QueryFirstOrDefault<UserResponse>(sp, para);
        return res;
    }

    public DbResponse UpdateUser(UserCommon user)
    {
        string sp = "spa_user_list";
        DynamicParameters para = new DynamicParameters();
        para.Add("@flag", 'u');
        para.Add("@user_id", user.UserId);
        para.Add("@username", user.Username);
        para.Add("@email", user.Email);
        para.Add("@gender", user.Gender);
        para.Add("@role", user.RoleId);

        try
        {
            var res = _context.Execute(sp, para);
            if (res > 0)
            {
                return new DbResponse { Status_Code = "0", Msg = "User Updated Successfully" };
            }
            else
            {
                return new DbResponse { Status_Code = "1", Msg = "User Update Failed" };
            }
        }
        catch (Exception ex)
        {
            return new DbResponse { Status_Code = "1", Msg = "Error occured while updating the user" };
        }
    }
}
