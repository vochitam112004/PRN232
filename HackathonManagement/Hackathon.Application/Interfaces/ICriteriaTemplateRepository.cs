using Hackathon.Domain.Entities;

namespace Hackathon.Application.Interfaces;

public interface ICriteriaTemplateRepository
{
    Task<IEnumerable<CriteriaTemplate>> GetAllWithItemsAsync();
    Task<CriteriaTemplate?> GetByIdWithItemsAsync(Guid id);
    Task AddAsync(CriteriaTemplate template);
    void Update(CriteriaTemplate template);
    void Delete(CriteriaTemplate template);
    void RemoveItems(IEnumerable<CriteriaTemplateItem> items);
    Task<CriteriaTemplateItem?> GetItemByIdAsync(Guid itemId);
    Task AddItemAsync(CriteriaTemplateItem item);
    void DeleteItem(CriteriaTemplateItem item);
    Task SaveChangesAsync();
}
