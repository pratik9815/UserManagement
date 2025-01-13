using DataLayer.Entities;

namespace UserManagement.Domain.Entities;

public class ApplicationUserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual ApplicationRole Role { get; set; }
}
