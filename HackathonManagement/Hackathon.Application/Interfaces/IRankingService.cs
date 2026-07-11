using System;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;

namespace Hackathon.Application.Interfaces;

public interface IRankingService
{
    Task<RoundRankingResponse> CalculateRankingAndAdvanceAsync(Guid roundId);
    Task<RoundRankingResponse> GetRoundResultsAsync(Guid roundId);
    Task<EventRankingResponse> GetEventRankingAsync(Guid eventId);
    Task<CategoryRankingResponse> GetCategoryRankingAsync(Guid roundId, Guid categoryId);
    Task<RoundScoreSummaryResponse> GetRoundScoreSummaryAsync(Guid roundId);
}

