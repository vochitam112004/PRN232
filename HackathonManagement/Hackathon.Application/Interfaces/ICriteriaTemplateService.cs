using Hackathon.Application.DTOs.CriteriaTemplate;

namespace Hackathon.Application.Interfaces;

public interface ICriteriaTemplateService
{
    Task<IEnumerable<CriteriaTemplateResponse>> GetAllAsync();
    Task<CriteriaTemplateResponse?> GetByIdAsync(Guid id);
    Task<CriteriaTemplateResponse> CreateAsync(Guid userId, CreateCriteriaTemplateRequest request);
    Task<(bool Success, string Error)> UpdateAsync(Guid id, UpdateCriteriaTemplateRequest request);
    Task<(bool Success, string Error)> UpdateItemsAsync(Guid id, UpdateCriteriaTemplateItemsRequest request);
    Task<(bool Success, string Error, CriteriaTemplateItemResponse? Item)> AddItemAsync(Guid templateId, CreateCriteriaTemplateItemDto request);
    Task<(bool Success, string Error)> UpdateItemAsync(Guid templateId, Guid itemId, CreateCriteriaTemplateItemDto request);
    Task<(bool Success, string Error)> DeleteItemAsync(Guid templateId, Guid itemId);
    Task<(bool Success, string Error)> DeleteAsync(Guid id);
}
