using Hackathon.Application.DTOs.Auth;

namespace Hackathon.Application.Interfaces;

public interface IAccountApprovalService
{
    Task<IEnumerable<PendingUserResponse>> GetPendingUsersAsync();
    Task<(bool Success, string Error)> ReviewAccountAsync(Guid reviewerId, ApprovalActionRequest request);
    Task<(bool Success, string Error)> CreateGuestJudgeAsync(CreateGuestJudgeRequest request);
    Task<(bool Success, string Error)> SuspendUserAsync(Guid userId, Guid reviewerId, string reason);
}
