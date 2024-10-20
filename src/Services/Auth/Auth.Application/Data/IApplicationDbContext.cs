

using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Data;

public interface IApplicationDbContext 
{
    DbSet<UserSession> UserSessions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
