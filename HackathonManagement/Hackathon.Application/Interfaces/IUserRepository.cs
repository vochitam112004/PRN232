using Hackathon.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hackathon.Application.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<ApplicationUser?> FindByIdAsync(Guid id);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> UpdateAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    Task<IEnumerable<ApplicationUser>> GetByStatusAsync(Domain.Enums.UserStatus status);
}
