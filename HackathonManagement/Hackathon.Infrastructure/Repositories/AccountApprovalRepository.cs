using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;

namespace Hackathon.Infrastructure.Repositories;

public class AccountApprovalRepository : IAccountApprovalRepository
{
    private readonly ApplicationDbContext _db;

    public AccountApprovalRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(AccountApproval approval)
        => await _db.AccountApprovals.AddAsync(approval);

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
