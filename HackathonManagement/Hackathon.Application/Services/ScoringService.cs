using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class ScoringService : IScoringService
{
    private readonly IJudgeScoreRepository _scoreRepository;
    private readonly IJudgeAssignmentRepository _assignmentRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IAuditLoggerService _auditLogger;

    public ScoringService(
        IJudgeScoreRepository scoreRepository,
        IJudgeAssignmentRepository assignmentRepository,
        ISubmissionRepository submissionRepository,
        IEventRepository eventRepository,
        IAuditLoggerService auditLogger)
    {
        _scoreRepository = scoreRepository;
        _assignmentRepository = assignmentRepository;
        _submissionRepository = submissionRepository;
        _eventRepository = eventRepository;
        _auditLogger = auditLogger;
    }

    public async Task SubmitScoresAsync(Guid judgeId, Guid submissionId, SubmitScoreRequest request)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId)
            ?? throw new KeyNotFoundException("Không tìm thấy bài nộp.");

        if (submission.IsDisqualified)
            throw new InvalidOperationException("Bài nộp này đã bị loại, không thể chấm điểm.");

        // Kiểm tra giám khảo có được phân công vào vòng này không
        var assignment = await _assignmentRepository.GetAsync(submission.RoundId, judgeId)
            ?? throw new UnauthorizedAccessException("Giám khảo không được phân công chấm vòng thi này.");

        // Validate thời gian chấm điểm — lấy Round từ Event
        var allEvents = await _eventRepository.GetAllAsync();
        var round = allEvents.SelectMany(e => e.Rounds)
                             .FirstOrDefault(r => r.Id == submission.RoundId)
            ?? throw new KeyNotFoundException("Không tìm thấy vòng thi.");

        var now = DateTime.UtcNow;
        if (round.JudgingStart.HasValue && now < round.JudgingStart.Value)
            throw new InvalidOperationException($"Chưa đến thời gian chấm điểm. Bắt đầu lúc: {round.JudgingStart.Value:yyyy-MM-dd HH:mm} UTC.");
        if (round.JudgingEnd.HasValue && now > round.JudgingEnd.Value)
            throw new InvalidOperationException("Đã hết thời gian chấm điểm cho vòng thi này.");

        // Validate EventCriteriaId thuộc event của vòng thi
        var parentEvent = allEvents.FirstOrDefault(e => e.Rounds.Any(r => r.Id == submission.RoundId));
        if (parentEvent != null)
        {
            var validCriteriaIds = parentEvent.Criteria.Where(c => c.IsActive).Select(c => c.Id).ToHashSet();
            var invalidIds = request.Scores.Where(s => !validCriteriaIds.Contains(s.EventCriteriaId)).ToList();
            if (invalidIds.Any())
                throw new ArgumentException($"Tiêu chí không hợp lệ hoặc không thuộc sự kiện: {string.Join(", ", invalidIds.Select(s => s.EventCriteriaId))}");
        }

        bool anyNew = false;

        foreach (var criteriaScore in request.Scores)
        {
            var existingScore = await _scoreRepository.GetScoreAsync(submissionId, judgeId, criteriaScore.EventCriteriaId);
            if (existingScore != null)
            {
                existingScore.Score = criteriaScore.Score;
                existingScore.Comment = criteriaScore.Comment;
                existingScore.UpdatedAt = DateTime.UtcNow;
                _scoreRepository.Update(existingScore);
            }
            else
            {
                var newScore = new JudgeScore
                {
                    SubmissionId = submissionId,
                    JudgeId = judgeId,
                    EventCriteriaId = criteriaScore.EventCriteriaId,
                    Score = criteriaScore.Score,
                    Comment = criteriaScore.Comment
                };
                await _scoreRepository.AddAsync(newScore);
                anyNew = true;
            }
        }

        await _scoreRepository.SaveChangesAsync();

        // Ghi AuditLog
        var auditAction = anyNew ? AuditAction.ScoreSubmitted : AuditAction.ScoreUpdated;
        await _auditLogger.LogAsync(
            action: auditAction,
            performedBy: judgeId,
            targetType: "submission",
            targetId: submissionId,
            payload: new { roundId = submission.RoundId, criteriaCount = request.Scores.Count });
    }

    public async Task<IEnumerable<SubmissionScoreResponse>> GetScoresForSubmissionAsync(Guid submissionId)
    {
        var rawScores = await _scoreRepository.GetBySubmissionAsync(submissionId);
        var grouped = rawScores.GroupBy(s => s.JudgeId);

        var responses = new List<SubmissionScoreResponse>();
        foreach (var group in grouped)
        {
            var first = group.First();
            var total = group.Sum(s => s.Score * (s.EventCriteria?.Weight ?? 1));

            responses.Add(new SubmissionScoreResponse
            {
                SubmissionId = submissionId,
                JudgeId = group.Key,
                JudgeName = first.Judge?.FullName ?? string.Empty,
                ScoredAt = first.ScoredAt,
                TotalScore = total,
                Scores = group.Select(s => new CriteriaScoreResponse
                {
                    EventCriteriaId = s.EventCriteriaId,
                    CriteriaName = s.EventCriteria?.Name ?? string.Empty,
                    Score = s.Score,
                    MaxScore = s.EventCriteria?.MaxScore ?? 0,
                    Weight = s.EventCriteria?.Weight ?? 1,
                    Comment = s.Comment
                }).ToList()
            });
        }
        return responses;
    }

    public async Task<SubmissionScoreResponse?> GetScoreByJudgeAsync(Guid submissionId, Guid judgeId)
    {
        var allScores = await GetScoresForSubmissionAsync(submissionId);
        return allScores.FirstOrDefault(s => s.JudgeId == judgeId);
    }
}
