using System;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;

namespace Hackathon.Application.Interfaces;

public interface IRankingService
{
    Task<RoundRankingResponse> CalculateRankingAndAdvanceAsync(Guid roundId);
    Task<RoundRankingResponse> GetRoundResultsAsync(Guid roundId);
}
