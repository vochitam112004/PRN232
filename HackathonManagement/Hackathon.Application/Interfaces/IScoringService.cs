using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;

namespace Hackathon.Application.Interfaces;

public interface IScoringService
{
    Task SubmitScoresAsync(Guid judgeId, Guid submissionId, SubmitScoreRequest request);
    Task<IEnumerable<SubmissionScoreResponse>> GetScoresForSubmissionAsync(Guid submissionId);
    Task<SubmissionScoreResponse?> GetScoreByJudgeAsync(Guid submissionId, Guid judgeId);
}
