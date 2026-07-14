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
        // 1. Lấy event chứa round để đọc PromotionRules
        var events = await _eventRepository.GetAllAsync();
        var parentEvent = events.FirstOrDefault(e => e.Rounds.Any(r => r.Id == roundId));
        var round = parentEvent?.Rounds.FirstOrDefault(r => r.Id == roundId);

        if (round == null || parentEvent == null)
            throw new KeyNotFoundException($"Không tìm thấy vòng thi {roundId}.");

        // 2. Lấy toàn bộ bài nộp hợp lệ (chưa bị loại)
        var submissions = (await _submissionRepository.GetSubmissionsByRoundAsync(roundId))
            .Where(s => !s.IsDisqualified)
            .ToList();

        // 3. Tính tổng điểm trọng số cho từng bài nộp (trung bình qua các giám khảo)
        var allScores = await _scoreRepository.GetByRoundAsync(roundId);
        var submissionTotals = ComputeSubmissionTotals(submissions, allScores);

        // 4. Xếp hạng theo Category riêng, sau đó tổng hợp
        var groupedByCategory = submissions
            .GroupBy(s => s.Team?.Category?.Name ?? "Không có hạng mục")
            .ToList();

        var results = new List<RoundResult>();

        foreach (var categoryGroup in groupedByCategory)
        {
            var categorySubmissions = categoryGroup
                .OrderByDescending(s => submissionTotals.GetValueOrDefault(s.Id, 0))
                .ToList();

            int categoryRank = 1;
            foreach (var submission in categorySubmissions)
            {
                results.Add(new RoundResult
                {
                    RoundId = roundId,
                    SubmissionId = submission.Id,
                    TotalScore = submissionTotals.GetValueOrDefault(submission.Id, 0),
                    Rank = categoryRank,
                    IsAdvanced = false, // sẽ xác định bên dưới
                    CalculatedAt = DateTime.UtcNow
                });
                categoryRank++;
            }
        }

        // 5. Xác định đội đi tiếp dựa trên PromotionRules
        ApplyPromotionRules(results, round.PromotionRules.ToList(), submissions);

        // 6. Lưu vào DB (xoá cũ, ghi mới)
        var existingResults = await _roundResultRepository.GetByRoundAsync(roundId);
        if (existingResults.Any())
            _roundResultRepository.RemoveRange(existingResults);

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
            }).OrderBy(r => r.Rank).ToList()
        };
    }

    public async Task<EventRankingResponse> GetEventRankingAsync(Guid eventId)
    {
        var ev = await _eventRepository.GetByIdWithDetailsAsync(eventId)
            ?? throw new KeyNotFoundException($"Không tìm thấy sự kiện {eventId}.");

        // Lấy vòng cuối cùng
        var finalRound = ev.Rounds.OrderByDescending(r => r.RoundOrder).FirstOrDefault();
        if (finalRound == null)
            return new EventRankingResponse { EventId = eventId, EventTitle = ev.Title };

        var roundResults = await GetRoundResultsAsync(finalRound.Id);

        return new EventRankingResponse
        {
            EventId = eventId,
            EventTitle = ev.Title,
            FinalRoundName = finalRound.Name,
            CalculatedAt = roundResults.CalculatedAt,
            Results = roundResults.Results
        };
    }

    public async Task<CategoryRankingResponse> GetCategoryRankingAsync(Guid roundId, Guid categoryId)
    {
        var results = await _roundResultRepository.GetByRoundAsync(roundId);
        var roundName = results.FirstOrDefault()?.Round?.Name ?? string.Empty;

        // Lọc kết quả của hạng mục được yêu cầu
        var categoryResults = results
            .Where(r => r.Submission?.Team?.CategoryId == categoryId)
            .OrderBy(r => r.Rank)
            .ToList();

        var categoryName = categoryResults.FirstOrDefault()?.Submission?.Team?.Category?.Name
            ?? string.Empty;

        return new CategoryRankingResponse
        {
            RoundId = roundId,
            RoundName = roundName,
            CategoryId = categoryId,
            CategoryName = categoryName,
            CalculatedAt = categoryResults.FirstOrDefault()?.CalculatedAt ?? DateTime.UtcNow,
            Results = categoryResults.Select(r => new TeamResultDto
            {
                SubmissionId = r.SubmissionId,
                TeamId = r.Submission?.TeamId ?? Guid.Empty,
                TeamName = r.Submission?.Team?.Name ?? string.Empty,
                CategoryName = categoryName,
                TotalScore = r.TotalScore,
                Rank = r.Rank,
                IsAdvanced = r.IsAdvanced,
                Note = r.Note
            }).ToList()
        };
    }

    public async Task<RoundScoreSummaryResponse> GetRoundScoreSummaryAsync(Guid roundId)
    {
        var allScores = await _scoreRepository.GetByRoundAsync(roundId);
        var submissions = await _submissionRepository.GetSubmissionsByRoundAsync(roundId);

        var events = await _eventRepository.GetAllAsync();
        var round = events.SelectMany(e => e.Rounds).FirstOrDefault(r => r.Id == roundId);
        var roundName = round?.Name ?? string.Empty;

        // Nhóm theo tiêu chí
        var byCriteria = allScores.GroupBy(s => s.EventCriteriaId).ToList();

        var criteriaSummaries = byCriteria.Select(group =>
        {
            var scores = group.Select(s => s.Score).ToList();
            var avg = scores.Average();
            var stdDev = scores.Count > 1
                ? (decimal)Math.Sqrt((double)scores.Select(s => (s - avg) * (s - avg)).Average())
                : 0;

            var first = group.First();
            return new CriteriaSummaryDto
            {
                EventCriteriaId = group.Key,
                CriteriaName = first.EventCriteria?.Name ?? string.Empty,
                MaxScore = first.EventCriteria?.MaxScore ?? 0,
                Weight = first.EventCriteria?.Weight ?? 0,
                AverageScore = Math.Round(avg, 2),
                MinScore = scores.Min(),
                MaxScoreGiven = scores.Max(),
                StdDeviation = Math.Round(stdDev, 2)
            };
        }).ToList();

        return new RoundScoreSummaryResponse
        {
            RoundId = roundId,
            RoundName = roundName,
            TotalSubmissions = submissions.Count(),
            TotalJudges = allScores.Select(s => s.JudgeId).Distinct().Count(),
            CriteriaSummaries = criteriaSummaries
        };
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    /// <summary>Tính tổng điểm trọng số trung bình (qua các giám khảo) cho từng bài nộp.</summary>
    private static Dictionary<Guid, decimal> ComputeSubmissionTotals(
        List<Submission> submissions,
        IEnumerable<JudgeScore> allScores)
    {
        var totals = new Dictionary<Guid, decimal>();
        var scoreList = allScores.ToList();

        foreach (var submission in submissions)
        {
            var submissionScores = scoreList.Where(s => s.SubmissionId == submission.Id).ToList();
            if (!submissionScores.Any())
            {
                totals[submission.Id] = 0;
                continue;
            }

            var judges = submissionScores.Select(s => s.JudgeId).Distinct().ToList();
            decimal totalAllJudges = 0;

            foreach (var judgeId in judges)
            {
                var judgeScores = submissionScores.Where(s => s.JudgeId == judgeId);
                decimal judgeTotal = judgeScores.Sum(s => s.Score * (s.EventCriteria?.Weight ?? 1));
                totalAllJudges += judgeTotal;
            }

            totals[submission.Id] = judges.Count > 0 ? totalAllJudges / judges.Count : 0;
        }

        return totals;
    }

    /// <summary>Áp dụng PromotionRules để đánh dấu IsAdvanced cho từng kết quả.</summary>
    private static void ApplyPromotionRules(
        List<RoundResult> results,
        List<RoundPromotionRule> rules,
        List<Submission> submissions)
    {
        if (!rules.Any())
        {
            // Không có rule: mặc định tất cả đều đi tiếp
            foreach (var r in results) r.IsAdvanced = true;
            return;
        }

        foreach (var rule in rules)
        {
            switch (rule.RuleType)
            {
                case PromotionRuleType.TopNPerCategory:
                    // Top N đội trong mỗi hạng mục
                    int topN = rule.TopN ?? 3;
                    var byCategory = results.GroupBy(r =>
                        submissions.FirstOrDefault(s => s.Id == r.SubmissionId)?.Team?.CategoryId);
                    foreach (var catGroup in byCategory)
                    {
                        foreach (var result in catGroup.OrderByDescending(r => r.TotalScore).Take(topN))
                            result.IsAdvanced = true;
                    }
                    break;

                case PromotionRuleType.TopNOverall:
                    // Top N đội toàn bộ (bất kể hạng mục)
                    int topNOverall = rule.TopN ?? 10;
                    foreach (var result in results.OrderByDescending(r => r.TotalScore).Take(topNOverall))
                        result.IsAdvanced = true;
                    break;

                case PromotionRuleType.ScoreThreshold:
                    // Đội nào đạt đủ điểm threshold
                    decimal threshold = rule.ScoreThreshold ?? 0;
                    foreach (var result in results.Where(r => r.TotalScore >= threshold))
                        result.IsAdvanced = true;
                    break;
            }
        }
    }
}
