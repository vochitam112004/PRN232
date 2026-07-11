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

    public async Task<(IEnumerable<ApplicationUser> Items, int TotalCount)> GetAllPagedAsync(
        UserStatus? status = null,
        string? role = null,
        int page = 1,
        int pageSize = 20)
    {
        var query = _userManager.Users
            .Include(u => u.StudentProfile)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(u => u.Status == status.Value);

        // Filter by role requires a cross-join with AspNetUserRoles — use subquery via UserManager
        IQueryable<ApplicationUser> finalQuery = query;
        if (!string.IsNullOrWhiteSpace(role))
        {
            // Get users in role as a set of IDs first
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Trim().ToLower());
            var roleUserIds = usersInRole.Select(u => u.Id).ToHashSet();
            finalQuery = query.Where(u => roleUserIds.Contains(u.Id));
        }

        var totalCount = await finalQuery.CountAsync();
        var items = await finalQuery
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
