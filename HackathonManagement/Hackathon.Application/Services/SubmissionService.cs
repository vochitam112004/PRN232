using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Submission;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class SubmissionService : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepo;
    private readonly ITeamRepository _teamRepo;
    private readonly IEventRepository _eventRepo;
    private readonly IUserRepository _userRepo;
    private readonly IGitMetadataService _gitMetadataService;

    public SubmissionService(
        ISubmissionRepository submissionRepo,
        ITeamRepository teamRepo,
        IEventRepository eventRepo,
        IUserRepository userRepo,
        IGitMetadataService gitMetadataService)
    {
        _submissionRepo = submissionRepo;
        _teamRepo = teamRepo;
        _eventRepo = eventRepo;
        _userRepo = userRepo;
        _gitMetadataService = gitMetadataService;
    }

    public async Task<SubmissionResponse> SubmitProjectAsync(Guid userId, CreateSubmissionRequest request)
    {
        var user = await _userRepo.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("Không tìm thấy người dùng.");

        if (user.Status != UserStatus.Approved)
            throw new InvalidOperationException("Tài khoản của bạn chưa được phê duyệt hoặc đang bị khóa.");

        // Find the round and its parent event
        var allEvents = await _eventRepo.GetAllAsync();
        Round? round = null;
        Event? parentEvent = null;
        foreach (var e in allEvents)
        {
            var r = e.Rounds.FirstOrDefault(x => x.Id == request.RoundId);
            if (r != null)
            {
                round = r;
                parentEvent = e;
                break;
            }
        }

        if (round == null || parentEvent == null)
            throw new ArgumentException("Vòng thi không tồn tại.");

        // Validate submission timeframe
        var now = DateTime.UtcNow;
        if (round.SubmissionStart.HasValue && now < round.SubmissionStart.Value)
            throw new InvalidOperationException($"Thời gian nộp bài cho vòng thi này chưa bắt đầu (Bắt đầu lúc: {round.SubmissionStart.Value.ToLocalTime()}).");
        if (now > round.SubmissionDeadline)
            throw new InvalidOperationException("Đã quá hạn nộp bài cho vòng thi này.");

        // Find the user's team in this event
        var team = await _teamRepo.GetByStudentIdInEventAsync(userId, parentEvent.Id);
        if (team == null)
            throw new InvalidOperationException("Bạn không thuộc đội thi nào trong sự kiện này.");

        // Reload team with full members list to validate size
        var detailedTeam = await _teamRepo.GetByIdWithMembersAsync(team.Id);
        if (detailedTeam == null)
            throw new ArgumentException("Không thể tìm thấy thông tin chi tiết đội thi.");

        if (detailedTeam.Members.Count < 3)
            throw new InvalidOperationException($"Đội thi của bạn hiện tại mới có {detailedTeam.Members.Count} thành viên. Quy định đội phải có từ 3 đến 5 thành viên mới được nộp bài.");

        // Check if submission already exists
        var submission = await _submissionRepo.GetByTeamAndRoundAsync(detailedTeam.Id, request.RoundId);
        bool isNew = submission == null;

        if (submission == null)
        {
            submission = new Submission
            {
                Id = Guid.NewGuid(),
                TeamId = detailedTeam.Id,
                RoundId = request.RoundId,
                CreatedAt = DateTime.UtcNow
            };
        }

        submission.RepoUrl = request.RepoUrl.Trim();
        submission.DemoUrl = request.DemoUrl?.Trim();
        submission.VideoUrl = request.VideoUrl?.Trim();
        submission.Description = request.Description?.Trim();
        submission.SubmittedAt = DateTime.UtcNow;
        submission.UpdatedAt = DateTime.UtcNow;

        // Fetch optional git repository metadata if it's a github URL
        var (desc, stars, msg, commitDate, lang) = await _gitMetadataService.GetRepoMetadataAsync(request.RepoUrl);
        submission.RepoDescription = desc;
        submission.RepoStars = stars;
        submission.RepoLastCommitMessage = msg;
        submission.RepoLastCommitDate = commitDate;
        submission.RepoPrimaryLanguage = lang;

        if (isNew)
        {
            await _submissionRepo.AddAsync(submission!);
        }
        else
        {
            _submissionRepo.Update(submission!);
        }

        await _submissionRepo.SaveChangesAsync();

        // Reload submission details for mapping
        var savedSubmission = await _submissionRepo.GetByIdAsync(submission!.Id);
        return MapToSubmissionResponse(savedSubmission!);
    }

    public async Task<SubmissionResponse?> GetSubmissionByIdAsync(Guid submissionId)
    {
        var sub = await _submissionRepo.GetByIdAsync(submissionId);
        if (sub == null) return null;
        return MapToSubmissionResponse(sub);
    }

    public async Task<IEnumerable<SubmissionResponse>> GetTeamSubmissionsAsync(Guid teamId)
    {
        var subs = await _submissionRepo.GetSubmissionsByTeamAsync(teamId);
        return subs.Select(MapToSubmissionResponse);
    }

    public async Task<IEnumerable<SubmissionResponse>> GetRoundSubmissionsAsync(Guid roundId)
    {
        var subs = await _submissionRepo.GetSubmissionsByRoundAsync(roundId);
        return subs.Select(MapToSubmissionResponse);
    }

    private static SubmissionResponse MapToSubmissionResponse(Submission sub)
    {
        return new SubmissionResponse
        {
            Id = sub.Id,
            TeamId = sub.TeamId,
            TeamName = sub.Team.Name,
            RoundId = sub.RoundId,
            RoundName = sub.Round.Name,
            RepoUrl = sub.RepoUrl,
            DemoUrl = sub.DemoUrl,
            VideoUrl = sub.VideoUrl,
            Description = sub.Description,
            RepoDescription = sub.RepoDescription,
            RepoStars = sub.RepoStars,
            RepoLastCommitMessage = sub.RepoLastCommitMessage,
            RepoLastCommitDate = sub.RepoLastCommitDate,
            RepoPrimaryLanguage = sub.RepoPrimaryLanguage,
            SubmittedAt = sub.SubmittedAt,
            CreatedAt = sub.CreatedAt,
            UpdatedAt = sub.UpdatedAt
        };
    }
}
