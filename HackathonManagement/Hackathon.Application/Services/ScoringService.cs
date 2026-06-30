using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Services;

public class ScoringService : IScoringService
{
    private readonly IJudgeScoreRepository _scoreRepository;
    private readonly IJudgeAssignmentRepository _assignmentRepository;
    private readonly ISubmissionRepository _submissionRepository;

    public ScoringService(
        IJudgeScoreRepository scoreRepository,
        IJudgeAssignmentRepository assignmentRepository,
        ISubmissionRepository submissionRepository)
    {
        _scoreRepository = scoreRepository;
        _assignmentRepository = assignmentRepository;
        _submissionRepository = submissionRepository;
    }

    public async Task SubmitScoresAsync(Guid judgeId, Guid submissionId, SubmitScoreRequest request)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null) throw new Exception("Không tìm thấy bài nộp.");

        // Check if judge is assigned to this round
        var assignment = await _assignmentRepository.GetAsync(submission.RoundId, judgeId);
        if (assignment == null) throw new Exception("Giám khảo không được phân công chấm vòng thi này.");

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
            }
        }

        await _scoreRepository.SaveChangesAsync();
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
