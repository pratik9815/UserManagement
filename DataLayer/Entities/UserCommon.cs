using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;
public class UserCommon
{
    public int UserId { get; set; }
    public string Msg { get; set; }
    public string Code { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string LastLogin { get; set; }   
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
    public string RoleId { get; set; }    
}
public class UserResponse
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
    public string RoleId { get; set; }
}
