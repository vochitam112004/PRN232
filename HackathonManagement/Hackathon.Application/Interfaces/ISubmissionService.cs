using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Submission;

namespace Hackathon.Application.Interfaces;

public interface ISubmissionService
{
    Task<SubmissionResponse> SubmitProjectAsync(Guid userId, CreateSubmissionRequest request);
    Task<SubmissionResponse?> GetSubmissionByIdAsync(Guid submissionId);
    Task<IEnumerable<SubmissionResponse>> GetTeamSubmissionsAsync(Guid teamId);
    Task<IEnumerable<SubmissionResponse>> GetRoundSubmissionsAsync(Guid roundId);
}
