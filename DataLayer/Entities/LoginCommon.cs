namespace UserManagement.Domain.Entities;

public class LoginCommon
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string IpAddress { get; set; }

    public string PasswordHash { get; set; }
    public string BrowserInfo { get; set; }
    public string SessionId { get; set; }
    public string IpInfo { get; set; }
    public string IpCountry { get; set; }
}
