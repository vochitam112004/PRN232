using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<ApplicationUser?> FindByEmailAsync(string email)
        => _userManager.FindByEmailAsync(email);

    public Task<ApplicationUser?> FindByIdAsync(Guid id)
        => _userManager.FindByIdAsync(id.ToString());

    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        => _userManager.CreateAsync(user, password);

    public Task<IdentityResult> UpdateAsync(ApplicationUser user)
        => _userManager.UpdateAsync(user);

    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => _userManager.CheckPasswordAsync(user, password);

    public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        => _userManager.GetRolesAsync(user);

    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        => _userManager.AddToRoleAsync(user, role);

    public async Task<IEnumerable<ApplicationUser>> GetByStatusAsync(UserStatus status)
        => await _userManager.Users
            .Where(u => u.Status == status)
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
}
