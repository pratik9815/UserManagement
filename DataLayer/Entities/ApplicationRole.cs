namespace UserManagement.Domain.Entities;

public class ApplicationRole
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}
