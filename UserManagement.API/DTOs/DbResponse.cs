namespace UserManagement.Application.DTOs;

public class DbResponse
{
    public int ErrorCode { get; set; }
    public string Msg { get; set; }
    public string Status_Code { get; set; }
    public List<dynamic> data { get; set; }
}
public class RoleResponse
{
    public int id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
