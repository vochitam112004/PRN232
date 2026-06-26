using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class RankingService : IRankingService
{
    private readonly IRoundResultRepository _roundResultRepository;
    private readonly IJudgeScoreRepository _scoreRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IEventRepository _eventRepository;

    public RankingService(
        IRoundResultRepository roundResultRepository,
        IJudgeScoreRepository scoreRepository,
        ISubmissionRepository submissionRepository,
        IEventRepository eventRepository)
    {
        _roundResultRepository = roundResultRepository;
        _scoreRepository = scoreRepository;
        _submissionRepository = submissionRepository;
        _eventRepository = eventRepository;
    }

    public async Task<RoundRankingResponse> CalculateRankingAndAdvanceAsync(Guid roundId)
    {
        // 1. Get all submissions for the round
        var submissions = await _submissionRepository.GetSubmissionsByRoundAsync(roundId);
        
        // 2. Get all scores for the round
        var allScores = await _scoreRepository.GetByRoundAsync(roundId);
        
        // 3. Compute total score per submission (average across judges)
        var submissionTotals = new Dictionary<Guid, decimal>();
        foreach (var submission in submissions)
        {
            var submissionScores = allScores.Where(s => s.SubmissionId == submission.Id).ToList();
            if (!submissionScores.Any())
            {
                submissionTotals[submission.Id] = 0;
                continue;
            }

            // Calculate total weighted score per judge, then average across all judges
            var judges = submissionScores.Select(s => s.JudgeId).Distinct().ToList();
            decimal totalAllJudges = 0;
            
            foreach (var judgeId in judges)
            {
                var judgeScores = submissionScores.Where(s => s.JudgeId == judgeId);
                decimal judgeTotal = judgeScores.Sum(s => s.Score * (s.EventCriteria?.Weight ?? 1));
                totalAllJudges += judgeTotal;
            }

            submissionTotals[submission.Id] = totalAllJudges / judges.Count;
        }

        // 4. Sort and assign ranks
        var orderedSubmissions = submissionTotals.OrderByDescending(x => x.Value).ToList();
        
        var results = new List<RoundResult>();
        int currentRank = 1;
        foreach (var item in orderedSubmissions)
        {
            var submission = submissions.First(s => s.Id == item.Key);
            var isAdvanced = false; // Add actual promotion rule logic here later if needed
            
            // For now, let's say Top 10 advanced just as an example if we don't have promotion rule evaluated
            if (currentRank <= 10) isAdvanced = true;

            results.Add(new RoundResult
            {
                RoundId = roundId,
                SubmissionId = submission.Id,
                TotalScore = item.Value,
                Rank = currentRank,
                IsAdvanced = isAdvanced,
                CalculatedAt = DateTime.UtcNow
            });
            currentRank++;
        }

        // 5. Save to database
        var existingResults = await _roundResultRepository.GetByRoundAsync(roundId);
        if (existingResults.Any())
        {
            _roundResultRepository.RemoveRange(existingResults);
        }
        await _roundResultRepository.AddRangeAsync(results);
        await _roundResultRepository.SaveChangesAsync();

        return await GetRoundResultsAsync(roundId);
    }

    public async Task<RoundRankingResponse> GetRoundResultsAsync(Guid roundId)
    {
        var results = await _roundResultRepository.GetByRoundAsync(roundId);
        
        return new RoundRankingResponse
        {
            RoundId = roundId,
            RoundName = results.FirstOrDefault()?.Round?.Name ?? string.Empty,
            CalculatedAt = results.FirstOrDefault()?.CalculatedAt ?? DateTime.UtcNow,
            Results = results.Select(r => new TeamResultDto
            {
                SubmissionId = r.SubmissionId,
                TeamId = r.Submission?.TeamId ?? Guid.Empty,
                TeamName = r.Submission?.Team?.Name ?? string.Empty,
                CategoryName = r.Submission?.Team?.Category?.Name ?? string.Empty,
                TotalScore = r.TotalScore,
                Rank = r.Rank,
                IsAdvanced = r.IsAdvanced,
                Note = r.Note
            }).ToList()
        };
    }
}
