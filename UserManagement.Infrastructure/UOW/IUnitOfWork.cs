using DataLayer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using UserManagement.Infrastructure.IRepositories;

namespace UserManagement.Infrastructure.UOW;

public interface IUnitOfWork
{
    //IRepository<ApplicationUser> Users { get; }
    //IRepository<IdentityRole> Roles { get; }
    //Task<int> CompleteAsync(); // Commit changes

    IDataLogin Login { get; }
}
