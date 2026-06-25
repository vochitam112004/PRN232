using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event?> GetByIdWithDetailsAsync(Guid id);
    Task AddAsync(Event @event);
    void Update(Event @event);
    void Delete(Event @event);
    void RemoveCriteria(IEnumerable<EventCriteria> criteria);
    void RemovePromotionRules(IEnumerable<RoundPromotionRule> rules);
    Task SaveChangesAsync();
}
