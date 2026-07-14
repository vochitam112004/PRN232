using System;
using System.Threading.Tasks;

namespace Hackathon.Application.Interfaces;

public interface IGitMetadataService
{
    Task<(string? Description, int? Stars, string? LastCommitMessage, DateTime? LastCommitDate, string? PrimaryLanguage)> GetRepoMetadataAsync(string repoUrl);
}
