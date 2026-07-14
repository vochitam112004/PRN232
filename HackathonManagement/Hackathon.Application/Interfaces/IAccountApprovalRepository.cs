using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IAccountApprovalRepository
{
    Task AddAsync(AccountApproval approval);
    Task SaveChangesAsync();
}
