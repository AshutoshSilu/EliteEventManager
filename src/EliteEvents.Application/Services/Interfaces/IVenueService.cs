using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Venue;

namespace EliteEvents.Application.Services.Interfaces;

public interface IVenueService
{
    Task<ApiResponse<VenueDto>> GetByIdAsync(int id);
    Task<ApiResponse<PagedResult<VenueListDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<VenueDto>> CreateAsync(VenueCreateDto dto);
    Task<ApiResponse<VenueDto>> UpdateAsync(int id, VenueUpdateDto dto);
    Task<ApiResponse> DeleteAsync(int id);
    Task<ApiResponse<IReadOnlyList<VenueListDto>>> GetFeaturedAsync(int count = 6);
    Task<ApiResponse<PagedResult<VenueListDto>>> SearchAsync(string searchTerm, int? minCapacity, int? maxCapacity, QueryParameters parameters);
    Task<ApiResponse<bool>> CheckAvailabilityAsync(int venueId, DateOnly date, string? startTime, string? endTime);
    Task<ApiResponse<IReadOnlyList<VenueAvailabilityDto>>> GetAvailabilityAsync(int venueId, DateOnly startDate, DateOnly endDate);
}
