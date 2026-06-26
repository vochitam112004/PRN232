using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;

namespace Hackathon.Application.Interfaces;

public interface IJudgeAssignmentService
{
    Task AssignJudgeToRoundAsync(Guid roundId, Guid judgeId);
    Task RemoveJudgeFromRoundAsync(Guid roundId, Guid judgeId);
    Task<IEnumerable<JudgeAssignmentResponse>> GetAssignmentsByRoundAsync(Guid roundId);
    Task<IEnumerable<JudgeAssignmentResponse>> GetAssignmentsByJudgeAsync(Guid judgeId);
}
