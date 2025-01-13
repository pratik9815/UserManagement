namespace UserManagement.Infrastructure.IRepositories;

public interface IDataLogin
{
    void InsertRefreshToken(string protectedTicket, string username);
    int CheckIfRefreshTokenIsValid(string refreshToken, string username);
    void RevokeThisRefreshToken(string authTokenID);
}
