using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Review;

namespace EliteEvents.Application.Services.Interfaces;

public interface IReviewService
{
    Task<ApiResponse<ReviewDto>> GetByIdAsync(int id);
    Task<ApiResponse<PagedResult<ReviewDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<ReviewDto>> CreateAsync(ReviewCreateDto dto, int customerId);
    Task<ApiResponse<ReviewDto>> ReplyAsync(ReviewReplyDto dto, Guid repliedBy);
    Task<ApiResponse> ApproveAsync(int id);
    Task<ApiResponse> DeleteAsync(int id);
    Task<ApiResponse<IReadOnlyList<ReviewDto>>> GetByEntityAsync(string entityType, int entityId);
    Task<ApiResponse<IReadOnlyList<ReviewDto>>> GetByCustomerAsync(int customerId);
    Task<ApiResponse<IReadOnlyList<ReviewDto>>> GetPendingAsync();
}
