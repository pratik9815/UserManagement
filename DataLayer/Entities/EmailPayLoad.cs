namespace UserManagement.Domain.Entities;
public class EmailPayLoad
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string EmailToFullName { get; set; }
}
