using System.Reflection;
using Auth.Application.Data;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }


    public DbSet<UserSession> UserSessions => Set<UserSession>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<IdentityUserRole<string>>().ToTable("tbl.UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("tbl.UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("tbl.UserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("tbl.UserToken");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("tbl.RoleClaims");
        
    }
}