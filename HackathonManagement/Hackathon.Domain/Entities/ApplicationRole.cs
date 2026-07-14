using Microsoft.AspNetCore.Identity;

namespace Hackathon.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

