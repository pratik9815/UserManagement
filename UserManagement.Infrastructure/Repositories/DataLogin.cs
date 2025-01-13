using Dapper;
using UserManagement.Infrastructure.DataContext;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Infrastructure.Repositories;

public class DataLogin : IDataLogin
{
    private readonly DapperContext _context;
    public DataLogin(DapperContext context)
    {
        _context = context;
    }
    public void InsertRefreshToken(string protectedTicket, string username)
    {
        DynamicParameters paramToInsertRefreshToken = new DynamicParameters();
        paramToInsertRefreshToken.Add("@token_user_id", username);
        paramToInsertRefreshToken.Add("@protectedticket", protectedTicket);
        paramToInsertRefreshToken.Add("@Create_ts", DateTime.UtcNow);
        paramToInsertRefreshToken.Add("@expire_ts", DateTime.UtcNow.AddHours(1));
        string sqlToInsertRefreshToken = $"insert into authtoken(AUTHORIZATION_TYPE,token_user_id,protectedticket,create_ts,expire_ts) values ('RefreshToken',@token_user_id,@protectedticket,@Create_ts,@expire_ts)";
        _context.Execute(sqlToInsertRefreshToken, paramToInsertRefreshToken);
    }
    public int CheckIfRefreshTokenIsValid(string refreshToken, string username)
    {
        DynamicParameters param = new DynamicParameters();
        param.Add("@token_user_id", username);
        param.Add("@protectedticket", refreshToken);
        param.Add("@expire_ts", DateTime.UtcNow);
        param.Add("@AUTHORIZATION_TYPE", "RefreshToken");
        string sql = $"select tokenid from  authtoken where token_user_id =@token_user_id and protectedticket = @protectedticket and expire_ts > @expire_ts and AUTHORIZATION_TYPE = @AUTHORIZATION_TYPE";
        return _context.QueryFirstOrDefault<int>(sql, param);
    }

    public void RevokeThisRefreshToken(string authTokenID)
    {
        DynamicParameters paramToDeleteRefreshToken = new();
        paramToDeleteRefreshToken.Add("@auth_type", "Revoked");
        paramToDeleteRefreshToken.Add("@authTokenID", authTokenID);
        string sqlToDeletePreviousRefreshToken = $"update authtoken SET AUTHORIZATION_TYPE=@auth_type where TokenID=@authTokenID";
        _context.Execute(sqlToDeletePreviousRefreshToken, paramToDeleteRefreshToken);
    }
}
