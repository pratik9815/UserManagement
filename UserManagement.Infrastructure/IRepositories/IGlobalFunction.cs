using System.Security.Claims;

namespace UserManagement.Infrastructure.IRepositories;

public interface IGlobalFunction
{
    string GetIp();
    string GetBrowser();
    string Encrypt(string clearText);
    string Decrypt(string cipherText);
    //string GetUser();
    ClaimsPrincipal GetPrinciple(string token);
    string GetClaims(string key);
    string GetUser();
}
