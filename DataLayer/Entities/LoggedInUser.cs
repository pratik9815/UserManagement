namespace UserManagement.Domain.Entities;
public class LoggedInUser
{
    public int UserId { get; set; } 
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string IpAddress { get; set; }   
    public DateTime LoginTime { get; set; }
    public string Browser {  get; set; }
    public int SessionTimeOutPeriod { get; set; }
    public DateTime LastLoginTime { get; set; }
    public string SessionId { get; set; }

}
