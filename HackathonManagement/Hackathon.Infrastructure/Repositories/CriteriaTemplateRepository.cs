using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class CriteriaTemplateRepository : ICriteriaTemplateRepository
{
    private readonly ApplicationDbContext _db;

    public CriteriaTemplateRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CriteriaTemplate>> GetAllWithItemsAsync()
    {
        return await _db.CriteriaTemplates
            .Include(t => t.Items)
            .ToListAsync();
    }

    public async Task<CriteriaTemplate?> GetByIdWithItemsAsync(Guid id)
    {
        return await _db.CriteriaTemplates
            .Include(t => t.Items)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(CriteriaTemplate template)
    {
        await _db.CriteriaTemplates.AddAsync(template);
    }

    public void Update(CriteriaTemplate template)
    {
        _db.CriteriaTemplates.Update(template);
    }

    public void Delete(CriteriaTemplate template)
    {
        _db.CriteriaTemplates.Remove(template);
    }

    public void RemoveItems(IEnumerable<CriteriaTemplateItem> items)
    {
        _db.CriteriaTemplateItems.RemoveRange(items);
    }

    public async Task<CriteriaTemplateItem?> GetItemByIdAsync(Guid itemId)
    {
        return await _db.CriteriaTemplateItems.FindAsync(itemId);
    }

    public async Task AddItemAsync(CriteriaTemplateItem item)
    {
        await _db.CriteriaTemplateItems.AddAsync(item);
    }

    public void DeleteItem(CriteriaTemplateItem item)
    {
        _db.CriteriaTemplateItems.Remove(item);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
