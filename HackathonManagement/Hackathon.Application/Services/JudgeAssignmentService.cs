using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Application.DTOs.Scoring;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Services;

public class JudgeAssignmentService : IJudgeAssignmentService
{
    private readonly IJudgeAssignmentRepository _judgeAssignmentRepository;
    private readonly IEventRepository _eventRepository; // To check round existence if needed
    private readonly IUserRepository _userRepository;

    public JudgeAssignmentService(
        IJudgeAssignmentRepository judgeAssignmentRepository,
        IEventRepository eventRepository,
        IUserRepository userRepository)
    {
        _judgeAssignmentRepository = judgeAssignmentRepository;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
    }

    public async Task AssignJudgeToRoundAsync(Guid roundId, Guid judgeId)
    {
        var existing = await _judgeAssignmentRepository.GetAsync(roundId, judgeId);
        if (existing != null)
        {
            throw new Exception("Giám khảo đã được phân công vào vòng thi này.");
        }

        var assignment = new JudgeAssignment
        {
            RoundId = roundId,
            JudgeId = judgeId,
            AssignedAt = DateTime.UtcNow
        };

        await _judgeAssignmentRepository.AddAsync(assignment);
        await _judgeAssignmentRepository.SaveChangesAsync();
    }

    public async Task RemoveJudgeFromRoundAsync(Guid roundId, Guid judgeId)
    {
        var existing = await _judgeAssignmentRepository.GetAsync(roundId, judgeId);
        if (existing == null)
        {
            throw new Exception("Không tìm thấy phân công giám khảo này.");
        }

        _judgeAssignmentRepository.Remove(existing);
        await _judgeAssignmentRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<JudgeAssignmentResponse>> GetAssignmentsByRoundAsync(Guid roundId)
    {
        var assignments = await _judgeAssignmentRepository.GetByRoundAsync(roundId);
        return assignments.Select(a => new JudgeAssignmentResponse
        {
            RoundId = a.RoundId,
            RoundName = a.Round?.Name ?? string.Empty,
            JudgeId = a.JudgeId,
            JudgeName = a.Judge?.FullName ?? string.Empty,
            JudgeEmail = a.Judge?.Email ?? string.Empty,
            AssignedAt = a.AssignedAt
        });
    }

    public async Task<IEnumerable<JudgeAssignmentResponse>> GetAssignmentsByJudgeAsync(Guid judgeId)
    {
        var assignments = await _judgeAssignmentRepository.GetByJudgeAsync(judgeId);
        return assignments.Select(a => new JudgeAssignmentResponse
        {
            RoundId = a.RoundId,
            RoundName = a.Round?.Name ?? string.Empty,
            JudgeId = a.JudgeId,
            JudgeName = a.Judge?.FullName ?? string.Empty,
            JudgeEmail = a.Judge?.Email ?? string.Empty,
            AssignedAt = a.AssignedAt
        });
    }
}
