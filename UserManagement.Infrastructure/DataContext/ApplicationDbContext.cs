using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace DataLayer.DataContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUserRole>()
             .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<ApplicationUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<ApplicationUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<ApplicationUserClaim>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserClaims)
            .HasForeignKey(uc => uc.UserId);



        modelBuilder.Entity<Customer>().HasKey(p => p.Id);
    }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; }
    public DbSet<ApplicationUserRole> UserRoles { get; set; }
    public DbSet<ApplicationClaim> Claims { get; set; }
    public DbSet<ApplicationUserClaim> UserClaims { get; set; }
}

