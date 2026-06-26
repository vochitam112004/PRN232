using Hackathon.Application.DTOs.Event;

namespace Hackathon.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventResponse>> GetAllEventsAsync();
    Task<EventResponse?> GetEventByIdAsync(Guid id);
    Task<EventResponse> CreateEventAsync(Guid userId, CreateEventRequest request);
    Task<(bool Success, string Error)> UpdateEventCriteriaAsync(Guid eventId, UpdateEventCriteriaRequest request);
    Task<(bool Success, string Error, CategoryResponse? Category)> CreateCategoryAsync(Guid eventId, CreateCategoryRequest request);
    Task<(bool Success, string Error, RoundResponse? Round, bool IsNew)> ConfigureRoundAsync(Guid eventId, ConfigureRoundRequest request);
    Task<(bool Success, string Error)> DeleteEventAsync(Guid id);
}
