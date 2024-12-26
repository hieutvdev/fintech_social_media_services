using Microsoft.EntityFrameworkCore;
using User.Domain.Models;

namespace User.Application.Data;

public interface IApplicationDbContext
{
    DbSet<UserInfo> UserInfos { get; }
    
    DbSet<UserType> UserTypes { get; }
    
    DbSet<Follow>  Follows { get; }
    
    DbSet<Block> Blocks { get; }
    
    DbSet<FriendRequest> FriendRequests { get; }
    
    DbSet<FriendShip> FriendShips { get; }
    
    DbSet<Photo> Photos { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}