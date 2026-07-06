using System.Linq;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class DisqualificationService : IDisqualificationService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IAuditLoggerService _auditLogger;
    private readonly IRankingService _rankingService;

    public DisqualificationService(
        ITeamRepository teamRepository, 
        ISubmissionRepository submissionRepository,
        IAuditLoggerService auditLogger,
        IRankingService rankingService)
    {
        _teamRepository = teamRepository;
        _submissionRepository = submissionRepository;
        _auditLogger = auditLogger;
        _rankingService = rankingService;
    }

    public async Task DisqualifyTeamAsync(Guid teamId, Guid performedBy, string reason, string? ipAddress = null)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Lý do loại không được để trống.", nameof(reason));

        var team = await _teamRepository.GetByIdAsync(teamId)
            ?? throw new KeyNotFoundException($"Không tìm thấy đội {teamId}.");

        if (team.IsDisqualified)
            throw new InvalidOperationException("Đội này đã bị loại trước đó.");

        team.IsDisqualified = true;
        team.DisqualifiedAt = DateTime.UtcNow;
        team.DisqualifiedBy = performedBy;
        team.DisqualifyReason = reason;
        team.UpdatedAt = DateTime.UtcNow;

        _teamRepository.Update(team);
        await _teamRepository.SaveChangesAsync();

        // Ghi nhật ký (bất biến)
        await _auditLogger.LogAsync(
            action: AuditAction.TeamDisqualified,
            performedBy: performedBy,
            targetType: "team",
            targetId: teamId,
            payload: new { teamName = team.Name },
            reason: reason,
            ipAddress: ipAddress);

        // Tính lại xếp hạng cho MỌI vòng đội này có bài nộp
        var submissions = await _submissionRepository.GetSubmissionsByTeamAsync(teamId);
        var roundIds = submissions.Select(s => s.RoundId).Distinct().ToList();
        foreach (var roundId in roundIds)
            await _rankingService.CalculateRankingAndAdvanceAsync(roundId);
    }

    public async Task DisqualifySubmissionAsync(Guid submissionId, Guid performedBy, string reason, string? ipAddress = null)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Lý do loại không được để trống.", nameof(reason));

        var submission = await _submissionRepository.GetByIdAsync(submissionId)
            ?? throw new KeyNotFoundException($"Không tìm thấy bài nộp {submissionId}.");

        if (submission.IsDisqualified)
            throw new InvalidOperationException("Bài nộp này đã bị loại trước đó.");

        submission.IsDisqualified = true;
        submission.DisqualifiedAt = DateTime.UtcNow;
        submission.DisqualifiedBy = performedBy;
        submission.DisqualifyReason = reason;
        submission.UpdatedAt = DateTime.UtcNow;

        _submissionRepository.Update(submission);
        await _submissionRepository.SaveChangesAsync();

        await _auditLogger.LogAsync(
            action: AuditAction.SubmissionDisqualified,
            performedBy: performedBy,
            targetType: "submission",
            targetId: submissionId,
            reason: reason,
            ipAddress: ipAddress);

        // Tính lại xếp hạng cho vòng của bài nộp này
        await _rankingService.CalculateRankingAndAdvanceAsync(submission.RoundId);
    }
}