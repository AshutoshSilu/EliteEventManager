using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Event;

namespace EliteEvents.Application.Services.Interfaces;

public interface IEventService
{
    Task<ApiResponse<EventDto>> GetByIdAsync(int id);
    Task<ApiResponse<PagedResult<EventListDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<EventDto>> CreateAsync(EventCreateDto dto, Guid organizerId);
    Task<ApiResponse<EventDto>> UpdateAsync(int id, EventUpdateDto dto);
    Task<ApiResponse> DeleteAsync(int id);
    Task<ApiResponse<IReadOnlyList<EventListDto>>> GetFeaturedAsync(int count = 6);
    Task<ApiResponse<IReadOnlyList<EventListDto>>> GetUpcomingAsync(int count = 10);
    Task<ApiResponse<IReadOnlyList<EventListDto>>> GetByCategoryAsync(int categoryId);
    Task<ApiResponse<PagedResult<EventListDto>>> SearchAsync(string searchTerm, QueryParameters parameters);
    Task<ApiResponse<IReadOnlyList<EventCategoryDto>>> GetCategoriesAsync();
    Task<ApiResponse<EventCategoryDto>> CreateCategoryAsync(EventCategoryDto dto);
    Task<ApiResponse<EventCategoryDto>> UpdateCategoryAsync(int id, EventCategoryDto dto);
    Task<ApiResponse> DeleteCategoryAsync(int id);
}
