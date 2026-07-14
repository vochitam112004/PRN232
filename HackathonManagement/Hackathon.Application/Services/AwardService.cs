using Hackathon.Application.DTOs.Award;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class AwardService : IAwardService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRankingService _rankingService;
    private readonly IAwardRepository _awardRepository;
    private readonly INotificationService _notificationService;   // THÊM

    public AwardService(
        IEventRepository eventRepository,
        IRankingService rankingService,
        IAwardRepository awardRepository,
        INotificationService notificationService)                 // THÊM
    {
        _eventRepository = eventRepository;
        _rankingService = rankingService;
        _awardRepository = awardRepository;
        _notificationService = notificationService;               // THÊM
    }

    public async Task<List<AwardSuggestionDto>> SuggestAsync(Guid eventId)
    {
        var ev = await _eventRepository.GetByIdWithDetailsAsync(eventId)
            ?? throw new KeyNotFoundException($"Không tìm thấy sự kiện {eventId}.");

        // Lấy vòng cuối cùng
        var finalRound = ev.Rounds.OrderByDescending(r => r.RoundOrder).FirstOrDefault();
        if (finalRound == null)
            return new List<AwardSuggestionDto>();

        var ranking = await _rankingService.GetRoundResultsAsync(finalRound.Id);

        // Map top 3 -> loại giải
        var suggestions = new List<AwardSuggestionDto>();
        foreach (var r in ranking.Results.OrderBy(x => x.Rank).Take(3))
        {
            var (type, name) = r.Rank switch
            {
                1 => (AwardType.FirstPlace, "Giải Nhất"),
                2 => (AwardType.SecondPlace, "Giải Nhì"),
                3 => (AwardType.ThirdPlace, "Giải Ba"),
                _ => (AwardType.HonorableMention, "Giải Khuyến khích")
            };

            suggestions.Add(new AwardSuggestionDto
            {
                Rank = r.Rank,
                TeamId = r.TeamId,
                TeamName = r.TeamName,
                CategoryName = r.CategoryName,
                TotalScore = r.TotalScore,
                SuggestedAwardType = type,
                SuggestedAwardName = name
            });
        }

        return suggestions;
    }

    public async Task GrantAsync(GrantAwardRequest request, Guid performedBy)
    {
        if (request.Items == null || request.Items.Count == 0)
            throw new ArgumentException("Danh sách trao giải trống.");

        var ev = await _eventRepository.GetByIdWithDetailsAsync(request.EventId)
            ?? throw new KeyNotFoundException($"Không tìm thấy sự kiện {request.EventId}.");

        foreach (var item in request.Items)
        {
            // Tạo giải
            var award = new Award
            {
                EventId = request.EventId,
                Name = item.Name,
                AwardType = item.AwardType,
                PrizeValue = item.PrizeValue,
                CategoryId = item.CategoryId
            };
            await _awardRepository.AddAsync(award);

            // Gán đội nhận giải
            await _awardRepository.AddRecipientAsync(new AwardRecipient
            {
                Award = award,          // EF tự nối AwardId qua navigation
                TeamId = item.TeamId,
                GrantedBy = performedBy,
                Note = item.Note
            });
        }

        await _awardRepository.SaveChangesAsync();
        // Tự động thông báo cho toàn bộ người tham gia sự kiện.
        // Bọc try-catch: nếu gửi thông báo lỗi cũng KHÔNG làm hỏng việc trao giải.
        try
        {
            await _notificationService.NotifyEventParticipantsAsync(
                request.EventId,
                "Kết quả đã được công bố",
                $"Ban tổ chức đã công bố kết quả và trao giải cho sự kiện \"{ev.Title}\". Xem chi tiết trên hệ thống.");
        }
        catch
        {
            // Có thể log lại ở đây nếu cần. Trao giải vẫn được coi là thành công.
        }
    }

    public async Task<IEnumerable<Award>> GetByEventAsync(Guid eventId)
        => await _awardRepository.GetByEventAsync(eventId);
}