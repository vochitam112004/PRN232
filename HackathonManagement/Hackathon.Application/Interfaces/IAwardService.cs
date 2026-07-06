using Hackathon.Application.DTOs.Award;

namespace Hackathon.Application.Interfaces;

public interface IAwardService
{
    // Gợi ý top 3 đội theo xếp hạng vòng cuối (chỉ đọc, không ghi)
    Task<List<AwardSuggestionDto>> SuggestAsync(Guid eventId);

    // BTC xác nhận -> tạo Award + AwardRecipient
    Task GrantAsync(GrantAwardRequest request, Guid performedBy);

    // Xem các giải đã trao của 1 sự kiện
    Task<IEnumerable<Domain.Entities.Award>> GetByEventAsync(Guid eventId);
}