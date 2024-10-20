using Microsoft.AspNetCore.Identity;
using ShredKernel.Aggregates;

namespace Auth.Domain.Models;

public class ApplicationUser : IdentityUser, IDateTracking
{
    public string AvatarUrl { get; set; } = default!;
    public int Status { get; set; } = 1;
    public string FullName { get; set; } = default!;
    public string BirthDay { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; } 
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; } 
    
    public virtual ICollection<UserSession> UserSessions { get; set; }
}