using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hackathon.Application.Interfaces;

namespace Hackathon.Infrastructure.Services;

public class GitMetadataService : IGitMetadataService
{
    public async Task<(string? Description, int? Stars, string? LastCommitMessage, DateTime? LastCommitDate, string? PrimaryLanguage)> GetRepoMetadataAsync(string repoUrl)
    {
        if (string.IsNullOrWhiteSpace(repoUrl))
            return (null, null, null, null, null);

        try
        {
            // Extract owner and repo name from GitHub URL
            // Match: github.com/owner/repo or github.com/owner/repo.git
            var match = Regex.Match(repoUrl, @"github\.com/([\w\-]+)/([\w\-]+)", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                // Not a GitHub URL, or doesn't match standard pattern. Return empty metadata gracefully.
                return (null, null, null, null, null);
            }

            var owner = match.Groups[1].Value;
            var repo = match.Groups[2].Value;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("HackathonManager", "1.0"));
            client.Timeout = TimeSpan.FromSeconds(5);

            // Fetch repo info
            var repoResponse = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}");
            if (!repoResponse.IsSuccessStatusCode)
                return (null, null, null, null, null);

            var repoJson = await repoResponse.Content.ReadAsStringAsync();
            using var repoDoc = JsonDocument.Parse(repoJson);
            var root = repoDoc.RootElement;

            string? description = root.TryGetProperty("description", out var descProp) ? descProp.GetString() : null;
            int? stars = root.TryGetProperty("stargazers_count", out var starsProp) ? starsProp.GetInt32() : null;
            string? primaryLanguage = root.TryGetProperty("language", out var langProp) ? langProp.GetString() : null;

            // Fetch last commit
            string? lastCommitMessage = null;
            DateTime? lastCommitDate = null;

            var commitsResponse = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/commits");
            if (commitsResponse.IsSuccessStatusCode)
            {
                var commitsJson = await commitsResponse.Content.ReadAsStringAsync();
                using var commitsDoc = JsonDocument.Parse(commitsJson);
                var commitsArray = commitsDoc.RootElement;
                if (commitsArray.ValueKind == JsonValueKind.Array && commitsArray.GetArrayLength() > 0)
                {
                    var lastCommit = commitsArray[0];
                    if (lastCommit.TryGetProperty("commit", out var commitDetail))
                    {
                        lastCommitMessage = commitDetail.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : null;
                        if (commitDetail.TryGetProperty("committer", out var committerDetail) && 
                            committerDetail.TryGetProperty("date", out var dateProp) && 
                            DateTime.TryParse(dateProp.GetString(), out var parsedDate))
                        {
                            lastCommitDate = parsedDate;
                        }
                    }
                }
            }

            return (description, stars, lastCommitMessage, lastCommitDate, primaryLanguage);
        }
        catch
        {
            // Fail gracefully if API is down, rate limited, or offline
            return (null, null, null, null, null);
        }
    }
}
