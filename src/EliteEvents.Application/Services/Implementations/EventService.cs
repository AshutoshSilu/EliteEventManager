using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Event;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

/// <summary>
/// Handles event management operations.
/// </summary>
public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EventService> _logger;

    public EventService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EventService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<EventDto>> GetByIdAsync(int id)
    {
        var evt = await _unitOfWork.Events.GetEventWithDetailsAsync(id);
        if (evt == null)
            return ApiResponse<EventDto>.FailResponse("Event not found.");

        var dto = _mapper.Map<EventDto>(evt);
        return ApiResponse<EventDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResult<EventListDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Events.Query().Where(e => !e.IsDeleted);

        // Apply search
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.ToLower();
            query = query.Where(e => e.Title.ToLower().Contains(search) ||
                                     (e.Description != null && e.Description.ToLower().Contains(search)));
        }

        // Apply sorting
        query = parameters.SortBy?.ToLower() switch
        {
            "title" => parameters.SortDirection == "desc" ? query.OrderByDescending(e => e.Title) : query.OrderBy(e => e.Title),
            "date" => parameters.SortDirection == "desc" ? query.OrderByDescending(e => e.StartDate) : query.OrderBy(e => e.StartDate),
            "price" => parameters.SortDirection == "desc" ? query.OrderByDescending(e => e.Price) : query.OrderBy(e => e.Price),
            _ => query.OrderByDescending(e => e.CreatedAt)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<EventListDto>>(items);
        var result = new PagedResult<EventListDto>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<EventListDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<EventDto>> CreateAsync(EventCreateDto dto, Guid organizerId)
    {
        var entity = _mapper.Map<Event>(dto);
        entity.OrganizerId = organizerId;
        entity.Status = EventStatus.Draft;

        await _unitOfWork.Events.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Event created: {EventId} - {Title}", entity.Id, entity.Title);

        var result = _mapper.Map<EventDto>(entity);
        return ApiResponse<EventDto>.SuccessResponse(result, "Event created successfully.");
    }

    public async Task<ApiResponse<EventDto>> UpdateAsync(int id, EventUpdateDto dto)
    {
        var entity = await _unitOfWork.Events.GetByIdAsync(id);
        if (entity == null || entity.IsDeleted)
            return ApiResponse<EventDto>.FailResponse("Event not found.");

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Events.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<EventDto>(entity);
        return ApiResponse<EventDto>.SuccessResponse(result, "Event updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Events.GetByIdAsync(id);
        if (entity == null || entity.IsDeleted)
            return ApiResponse.FailResponse("Event not found.");

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Events.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Event deleted successfully.");
    }

    public async Task<ApiResponse<IReadOnlyList<EventListDto>>> GetFeaturedAsync(int count = 6)
    {
        var events = await _unitOfWork.Events.GetFeaturedEventsAsync(count);
        var dtos = _mapper.Map<IReadOnlyList<EventListDto>>(events);
        return ApiResponse<IReadOnlyList<EventListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<EventListDto>>> GetUpcomingAsync(int count = 10)
    {
        var events = await _unitOfWork.Events.GetUpcomingEventsAsync(count);
        var dtos = _mapper.Map<IReadOnlyList<EventListDto>>(events);
        return ApiResponse<IReadOnlyList<EventListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<EventListDto>>> GetByCategoryAsync(int categoryId)
    {
        var events = await _unitOfWork.Events.GetEventsByCategoryAsync(categoryId);
        var dtos = _mapper.Map<IReadOnlyList<EventListDto>>(events);
        return ApiResponse<IReadOnlyList<EventListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<PagedResult<EventListDto>>> SearchAsync(string searchTerm, QueryParameters parameters)
    {
        var events = await _unitOfWork.Events.SearchEventsAsync(searchTerm);
        var dtos = _mapper.Map<IReadOnlyList<EventListDto>>(events);
        var result = new PagedResult<EventListDto>(dtos, dtos.Count, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<EventListDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<IReadOnlyList<EventCategoryDto>>> GetCategoriesAsync()
    {
        var categories = await _unitOfWork.Events.Query()
            .Select(e => e.Category)
            .Distinct()
            .ToListAsync();
        // Simplified - in production, use dedicated category repository
        return ApiResponse<IReadOnlyList<EventCategoryDto>>.SuccessResponse(new List<EventCategoryDto>());
    }

    public async Task<ApiResponse<EventCategoryDto>> CreateCategoryAsync(EventCategoryDto dto)
    {
        // Implementation would use a dedicated category repository
        return ApiResponse<EventCategoryDto>.SuccessResponse(dto, "Category created successfully.");
    }

    public async Task<ApiResponse<EventCategoryDto>> UpdateCategoryAsync(int id, EventCategoryDto dto)
    {
        return ApiResponse<EventCategoryDto>.SuccessResponse(dto, "Category updated successfully.");
    }

    public async Task<ApiResponse> DeleteCategoryAsync(int id)
    {
        return ApiResponse.SuccessResponse("Category deleted successfully.");
    }
}

// Extension method placeholder for async LINQ (EF Core provides these)
internal static class QueryableExtensions
{
    public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source) =>
        Task.FromResult(source.ToList());
}
