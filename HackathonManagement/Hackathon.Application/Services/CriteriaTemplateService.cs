using Hackathon.Application.DTOs.CriteriaTemplate;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Services;

public class CriteriaTemplateService : ICriteriaTemplateService
{
    private readonly ICriteriaTemplateRepository _templateRepo;

    public CriteriaTemplateService(ICriteriaTemplateRepository templateRepo)
    {
        _templateRepo = templateRepo;
    }

    public async Task<IEnumerable<CriteriaTemplateResponse>> GetAllAsync()
    {
        var templates = await _templateRepo.GetAllWithItemsAsync();
        return templates.Select(MapToResponse).ToList();
    }

    public async Task<CriteriaTemplateResponse?> GetByIdAsync(Guid id)
    {
        var template = await _templateRepo.GetByIdWithItemsAsync(id);
        if (template == null) return null;
        return MapToResponse(template);
    }

    public async Task<CriteriaTemplateResponse> CreateAsync(Guid userId, CreateCriteriaTemplateRequest request)
    {
        // If the new template is default, we should unmark any existing defaults
        if (request.IsDefault)
        {
            var existingTemplates = await _templateRepo.GetAllWithItemsAsync();
            foreach (var existing in existingTemplates)
            {
                if (existing.IsDefault)
                {
                    existing.IsDefault = false;
                }
            }
        }

        var template = new CriteriaTemplate
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            IsDefault = request.IsDefault,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        foreach (var item in request.Items)
        {
            template.Items.Add(new CriteriaTemplateItem
            {
                TemplateId = template.Id,
                Name = item.Name.Trim(),
                Description = item.Description?.Trim(),
                MaxScore = item.MaxScore,
                Weight = item.Weight,
                DisplayOrder = item.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _templateRepo.AddAsync(template);
        await _templateRepo.SaveChangesAsync();

        return MapToResponse(template);
    }

    public async Task<(bool Success, string Error)> UpdateAsync(Guid id, UpdateCriteriaTemplateRequest request)
    {
        var template = await _templateRepo.GetByIdWithItemsAsync(id);
        if (template == null)
            return (false, "Không tìm thấy mẫu tiêu chí.");

        // If updated to default, unmark other defaults
        if (request.IsDefault && !template.IsDefault)
        {
            var existingTemplates = await _templateRepo.GetAllWithItemsAsync();
            foreach (var existing in existingTemplates)
            {
                if (existing.IsDefault && existing.Id != id)
                {
                    existing.IsDefault = false;
                }
            }
        }

        template.Name = request.Name.Trim();
        template.Description = request.Description?.Trim();
        template.IsDefault = request.IsDefault;
        template.UpdatedAt = DateTime.UtcNow;

        await _templateRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error)> UpdateItemsAsync(Guid id, UpdateCriteriaTemplateItemsRequest request)
    {
        var template = await _templateRepo.GetByIdWithItemsAsync(id);
        if (template == null)
            return (false, "Không tìm thấy mẫu tiêu chí.");

        // Explicitly remove the old items from database
        var oldItems = template.Items.ToList();
        _templateRepo.RemoveItems(oldItems);

        template.Items.Clear();
        foreach (var item in request.Items)
        {
            template.Items.Add(new CriteriaTemplateItem
            {
                TemplateId = template.Id,
                Name = item.Name.Trim(),
                Description = item.Description?.Trim(),
                MaxScore = item.MaxScore,
                Weight = item.Weight,
                DisplayOrder = item.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            });
        }

        template.UpdatedAt = DateTime.UtcNow;
        await _templateRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error)> DeleteAsync(Guid id)
    {
        var template = await _templateRepo.GetByIdWithItemsAsync(id);
        if (template == null)
            return (false, "Không tìm thấy mẫu tiêu chí.");

        _templateRepo.Delete(template);
        await _templateRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error, CriteriaTemplateItemResponse? Item)> AddItemAsync(Guid templateId, CreateCriteriaTemplateItemDto request)
    {
        var template = await _templateRepo.GetByIdWithItemsAsync(templateId);
        if (template == null)
            return (false, "Không tìm thấy mẫu tiêu chí.", null);

        var item = new CriteriaTemplateItem
        {
            TemplateId = templateId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            MaxScore = request.MaxScore,
            Weight = request.Weight,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        template.Items.Add(item);
        template.UpdatedAt = DateTime.UtcNow;

        await _templateRepo.SaveChangesAsync();

        return (true, string.Empty, new CriteriaTemplateItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            MaxScore = item.MaxScore,
            Weight = item.Weight,
            DisplayOrder = item.DisplayOrder
        });
    }

    public async Task<(bool Success, string Error)> UpdateItemAsync(Guid templateId, Guid itemId, CreateCriteriaTemplateItemDto request)
    {
        var template = await _templateRepo.GetByIdWithItemsAsync(templateId);
        if (template == null)
            return (false, "Không tìm thấy mẫu tiêu chí.");

        var item = await _templateRepo.GetItemByIdAsync(itemId);
        if (item == null || item.TemplateId != templateId)
            return (false, "Không tìm thấy tiêu chí con trong mẫu này.");

        item.Name = request.Name.Trim();
        item.Description = request.Description?.Trim();
        item.MaxScore = request.MaxScore;
        item.Weight = request.Weight;
        item.DisplayOrder = request.DisplayOrder;

        template.UpdatedAt = DateTime.UtcNow;

        await _templateRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error)> DeleteItemAsync(Guid templateId, Guid itemId)
    {
        var template = await _templateRepo.GetByIdWithItemsAsync(templateId);
        if (template == null)
            return (false, "Không tìm thấy mẫu tiêu chí.");

        var item = await _templateRepo.GetItemByIdAsync(itemId);
        if (item == null || item.TemplateId != templateId)
            return (false, "Không tìm thấy tiêu chí con trong mẫu này.");

        _templateRepo.DeleteItem(item);
        template.UpdatedAt = DateTime.UtcNow;

        await _templateRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    private static CriteriaTemplateResponse MapToResponse(CriteriaTemplate t)
    {
        return new CriteriaTemplateResponse
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            IsDefault = t.IsDefault,
            CreatedBy = t.CreatedBy,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            Items = t.Items.Select(i => new CriteriaTemplateItemResponse
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                MaxScore = i.MaxScore,
                Weight = i.Weight,
                DisplayOrder = i.DisplayOrder
            }).OrderBy(i => i.DisplayOrder).ToList()
        };
    }
}
