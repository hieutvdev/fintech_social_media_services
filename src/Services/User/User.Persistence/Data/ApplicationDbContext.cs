using System.Reflection;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Domain.Models;

namespace User.Persistence.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserInfo> UserInfos => Set<UserInfo>();
    public DbSet<UserType> UserTypes => Set<UserType>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<Block> Blocks => Set<Block>();
    public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();
    public DbSet<FriendShip> FriendShips => Set<FriendShip>();
    public DbSet<Photo> Photos => Set<Photo>();
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}