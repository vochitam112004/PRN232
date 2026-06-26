using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _db;

    public EventRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _db.Events
            .Include(e => e.Criteria)
            .Include(e => e.Categories)
            .Include(e => e.Rounds)
                .ThenInclude(r => r.PromotionRules)
            .ToListAsync();
    }

    public async Task<Event?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _db.Events
            .Include(e => e.Criteria)
            .Include(e => e.Categories)
            .Include(e => e.Rounds)
                .ThenInclude(r => r.PromotionRules)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Event @event)
    {
        await _db.Events.AddAsync(@event);
    }

    public void Update(Event @event)
    {
        _db.Events.Update(@event);
    }

    public void Delete(Event @event)
    {
        _db.Events.Remove(@event);
    }

    public void RemoveCriteria(IEnumerable<EventCriteria> criteria)
    {
        _db.EventCriteria.RemoveRange(criteria);
    }

    public void RemovePromotionRules(IEnumerable<RoundPromotionRule> rules)
    {
        _db.RoundPromotionRules.RemoveRange(rules);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
