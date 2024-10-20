using Microsoft.AspNetCore.Identity;
using ShredKernel.Aggregates;

namespace Auth.Domain.Models;

public class ApplicationRole: IdentityRole, IDateTracking
{
    public string Descriptions { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}