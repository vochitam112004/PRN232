using Hackathon.Application.DTOs.Event;
using Hackathon.Application.Interfaces;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums;

namespace Hackathon.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepo;
    private readonly ICriteriaTemplateRepository _templateRepo;
    private readonly IUserRepository _userRepo;

    public EventService(
        IEventRepository eventRepo,
        ICriteriaTemplateRepository templateRepo,
        IUserRepository userRepo)
    {
        _eventRepo = eventRepo;
        _templateRepo = templateRepo;
        _userRepo = userRepo;
    }

    public async Task<IEnumerable<EventResponse>> GetAllEventsAsync()
    {
        var events = await _eventRepo.GetAllAsync();
        var responses = new List<EventResponse>();
        foreach (var ev in events)
        {
            responses.Add(await MapToResponseAsync(ev));
        }
        return responses;
    }

    public async Task<EventResponse?> GetEventByIdAsync(Guid id)
    {
        var ev = await _eventRepo.GetByIdWithDetailsAsync(id);
        if (ev == null) return null;
        return await MapToResponseAsync(ev);
    }

    public async Task<EventResponse> CreateEventAsync(Guid userId, CreateEventRequest request)
    {
        // Bug 8: Validate registration dates
        if (request.RegistrationStart.HasValue && request.RegistrationEnd.HasValue
            && request.RegistrationStart.Value >= request.RegistrationEnd.Value)
        {
            throw new ArgumentException("Ngày bắt đầu đăng ký phải trước ngày kết thúc đăng ký.");
        }

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            BannerUrl = request.BannerUrl?.Trim(),
            Status = EventStatus.Draft,
            RegistrationStart = request.RegistrationStart,
            RegistrationEnd = request.RegistrationEnd,
            CriteriaTemplateId = request.CriteriaTemplateId,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // If template is selected, copy all items to EventCriteria
        if (request.CriteriaTemplateId.HasValue)
        {
            var template = await _templateRepo.GetByIdWithItemsAsync(request.CriteriaTemplateId.Value);
            if (template != null)
            {
                foreach (var item in template.Items)
                {
                    // Bug 2: Let EF Core auto-generate Id for child entities
                    ev.Criteria.Add(new EventCriteria
                    {
                        EventId = ev.Id,
                        Name = item.Name,
                        Description = item.Description,
                        MaxScore = item.MaxScore,
                        Weight = item.Weight,
                        DisplayOrder = item.DisplayOrder,
                        IsActive = true,
                        SourceItemId = item.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        await _eventRepo.AddAsync(ev);
        await _eventRepo.SaveChangesAsync();

        return await MapToResponseAsync(ev);
    }

    public async Task<(bool Success, string Error)> UpdateEventCriteriaAsync(Guid eventId, UpdateEventCriteriaRequest request)
    {
        var ev = await _eventRepo.GetByIdWithDetailsAsync(eventId);
        if (ev == null)
            return (false, "Không tìm thấy sự kiện.");

        // Validate weights of active criteria
        var activeCriteria = request.Criteria.Where(c => c.IsActive).ToList();
        var totalWeight = activeCriteria.Sum(c => c.Weight);
        if (activeCriteria.Any() && Math.Abs(totalWeight - 1.0m) > 0.0001m)
        {
            return (false, $"Tổng trọng số của các tiêu chí đang kích hoạt phải bằng 1.0 (Hiện tại: {totalWeight}).");
        }

        // Bug 7: Validate that all provided Ids actually exist in this event
        var requestIds = request.Criteria.Where(c => c.Id.HasValue).Select(c => c.Id!.Value).ToList();
        foreach (var requestId in requestIds)
        {
            if (!ev.Criteria.Any(ec => ec.Id == requestId))
            {
                return (false, $"Không tìm thấy tiêu chí có Id: {requestId} trong sự kiện này.");
            }
        }

        // Find criteria to delete (existing ones not in the request)
        var criteriaToDelete = ev.Criteria.Where(c => !requestIds.Contains(c.Id)).ToList();
        _eventRepo.RemoveCriteria(criteriaToDelete);

        // Update existing or add new criteria
        foreach (var c in request.Criteria)
        {
            if (c.Id.HasValue)
            {
                var existing = ev.Criteria.FirstOrDefault(ec => ec.Id == c.Id.Value);
                // existing is guaranteed non-null due to validation above
                existing!.Name = c.Name.Trim();
                existing.Description = c.Description?.Trim();
                existing.MaxScore = c.MaxScore;
                existing.Weight = c.Weight;
                existing.DisplayOrder = c.DisplayOrder;
                existing.IsActive = c.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                ev.Criteria.Add(new EventCriteria
                {
                    EventId = ev.Id,
                    Name = c.Name.Trim(),
                    Description = c.Description?.Trim(),
                    MaxScore = c.MaxScore,
                    Weight = c.Weight,
                    DisplayOrder = c.DisplayOrder,
                    IsActive = c.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        ev.UpdatedAt = DateTime.UtcNow;
        await _eventRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error, CategoryResponse? Category)> CreateCategoryAsync(Guid eventId, CreateCategoryRequest request)
    {
        var ev = await _eventRepo.GetByIdWithDetailsAsync(eventId);
        if (ev == null)
            return (false, "Không tìm thấy sự kiện.", null);

        // Bug 15: Check duplicate category name within the same event
        if (ev.Categories.Any(c => c.Name.Equals(request.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
            return (false, "Hạng mục với tên này đã tồn tại trong sự kiện.", null);

        var category = new Category
        {
            EventId = eventId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            MaxTeams = request.MaxTeams,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        ev.Categories.Add(category);
        ev.UpdatedAt = DateTime.UtcNow;

        await _eventRepo.SaveChangesAsync();

        return (true, string.Empty, new CategoryResponse
        {
            Id = category.Id,
            EventId = category.EventId,
            Name = category.Name,
            Description = category.Description,
            MaxTeams = category.MaxTeams
        });
    }

    // Bug 10: Return IsNew flag so controller can return 201 vs 200
    public async Task<(bool Success, string Error, RoundResponse? Round, bool IsNew)> ConfigureRoundAsync(Guid eventId, ConfigureRoundRequest request)
    {
        var ev = await _eventRepo.GetByIdWithDetailsAsync(eventId);
        if (ev == null)
            return (false, "Không tìm thấy sự kiện.", null, false);

        // Bug 9: Validate round time constraints
        if (request.SubmissionStart.HasValue && request.SubmissionStart.Value >= request.SubmissionDeadline)
        {
            return (false, "Thời gian bắt đầu nộp bài phải trước deadline nộp bài.", null, false);
        }
        if (request.JudgingStart.HasValue && request.JudgingEnd.HasValue
            && request.JudgingStart.Value >= request.JudgingEnd.Value)
        {
            return (false, "Thời gian bắt đầu chấm điểm phải trước thời gian kết thúc chấm điểm.", null, false);
        }
        if (request.JudgingStart.HasValue && request.JudgingStart.Value < request.SubmissionDeadline)
        {
            return (false, "Thời gian bắt đầu chấm điểm phải sau deadline nộp bài.", null, false);
        }

        // Check if round already exists by order
        var round = ev.Rounds.FirstOrDefault(r => r.RoundOrder == request.RoundOrder);
        bool isNew = false;
        if (round == null)
        {
            isNew = true;
            round = new Round
            {
                EventId = eventId,
                RoundOrder = request.RoundOrder,
                CreatedAt = DateTime.UtcNow
            };
        }

        round.Name = request.Name.Trim();
        round.Description = request.Description?.Trim();
        round.SubmissionStart = request.SubmissionStart;
        round.SubmissionDeadline = request.SubmissionDeadline;
        round.JudgingStart = request.JudgingStart;
        round.JudgingEnd = request.JudgingEnd;
        round.IsCalibrationRound = request.IsCalibrationRound;
        round.UpdatedAt = DateTime.UtcNow;

        // Bug 3: Properly remove old promotion rules from database before clearing collection
        if (!isNew)
        {
            var oldRules = round.PromotionRules.ToList();
            _eventRepo.RemovePromotionRules(oldRules);
        }
        round.PromotionRules.Clear();

        foreach (var rule in request.PromotionRules)
        {
            if (!TryParsePromotionRuleType(rule.RuleType, out var ruleType))
            {
                return (false, $"Loại quy tắc thăng hạng không hợp lệ: {rule.RuleType}", null, false);
            }

            // Bug 4: Don't set RoundId explicitly — EF Core resolves FK via navigation property
            round.PromotionRules.Add(new RoundPromotionRule
            {
                RuleType = ruleType,
                TopN = rule.TopN,
                ScoreThreshold = rule.ScoreThreshold,
                PerCategory = rule.PerCategory,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (isNew)
        {
            ev.Rounds.Add(round);
        }

        ev.UpdatedAt = DateTime.UtcNow;
        await _eventRepo.SaveChangesAsync();

        return (true, string.Empty, MapRoundResponse(round), isNew);
    }

    public async Task<(bool Success, string Error)> DeleteEventAsync(Guid id)
    {
        var ev = await _eventRepo.GetByIdWithDetailsAsync(id);
        if (ev == null)
            return (false, "Không tìm thấy sự kiện.");

        _eventRepo.Delete(ev);
        await _eventRepo.SaveChangesAsync();

        return (true, string.Empty);
    }

    private async Task<EventResponse> MapToResponseAsync(Event ev)
    {
        var creatorUser = await _userRepo.FindByIdAsync(ev.CreatedBy);
        string creatorName = creatorUser?.FullName ?? "Unknown";

        return new EventResponse
        {
            Id = ev.Id,
            Title = ev.Title,
            Description = ev.Description,
            BannerUrl = ev.BannerUrl,
            // Bug 12: Use snake_case for consistent enum serialization
            Status = ToSnakeCaseString(ev.Status),
            RegistrationStart = ev.RegistrationStart,
            RegistrationEnd = ev.RegistrationEnd,
            CriteriaTemplateId = ev.CriteriaTemplateId,
            CreatedBy = ev.CreatedBy,
            CreatorName = creatorName,
            CreatedAt = ev.CreatedAt,
            UpdatedAt = ev.UpdatedAt,
            Criteria = ev.Criteria.Select(c => new EventCriteriaResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                MaxScore = c.MaxScore,
                Weight = c.Weight,
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive,
                SourceItemId = c.SourceItemId
            }).OrderBy(c => c.DisplayOrder).ToList(),
            Categories = ev.Categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                EventId = c.EventId,
                Name = c.Name,
                Description = c.Description,
                MaxTeams = c.MaxTeams
            }).ToList(),
            Rounds = ev.Rounds.Select(MapRoundResponse).OrderBy(r => r.RoundOrder).ToList()
        };
    }

    private static RoundResponse MapRoundResponse(Round r)
    {
        return new RoundResponse
        {
            Id = r.Id,
            EventId = r.EventId,
            Name = r.Name,
            Description = r.Description,
            RoundOrder = r.RoundOrder,
            // Bug 12: Use snake_case for consistent enum serialization
            Status = ToSnakeCaseString(r.Status),
            SubmissionStart = r.SubmissionStart,
            SubmissionDeadline = r.SubmissionDeadline,
            JudgingStart = r.JudgingStart,
            JudgingEnd = r.JudgingEnd,
            IsCalibrationRound = r.IsCalibrationRound,
            PromotionRules = r.PromotionRules.Select(pr => new PromotionRuleResponse
            {
                Id = pr.Id,
                // Bug 13: Use snake_case for consistent rule type serialization
                RuleType = ToSnakeCaseString(pr.RuleType),
                TopN = pr.TopN,
                ScoreThreshold = pr.ScoreThreshold,
                PerCategory = pr.PerCategory
            }).ToList()
        };
    }

    /// <summary>
    /// Converts PascalCase enum values to snake_case strings.
    /// E.g. OpenRegistration → "open_registration", TopNPerCategory → "top_n_per_category"
    /// </summary>
    private static string ToSnakeCaseString<T>(T val) where T : struct, Enum
    {
        var str = val.ToString();
        return string.Concat(str.Select((x, i) =>
            i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()
        )).ToLower();
    }

    private static bool TryParsePromotionRuleType(string input, out PromotionRuleType ruleType)
    {
        ruleType = PromotionRuleType.TopNOverall;
        var cleanInput = input.Replace("_", "").ToLower();
        if (cleanInput == "topnpercategory")
        {
            ruleType = PromotionRuleType.TopNPerCategory;
            return true;
        }
        if (cleanInput == "topnoverall")
        {
            ruleType = PromotionRuleType.TopNOverall;
            return true;
        }
        if (cleanInput == "scorethreshold")
        {
            ruleType = PromotionRuleType.ScoreThreshold;
            return true;
        }
        return false;
    }
}
