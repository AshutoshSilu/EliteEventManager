using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Review;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReviewService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<ReviewDto>> GetByIdAsync(int id)
    {
        var review = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (review == null)
            return ApiResponse<ReviewDto>.FailResponse("Review not found.");

        var dto = _mapper.Map<ReviewDto>(review);
        return ApiResponse<ReviewDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResult<ReviewDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Reviews.Query();

        query = parameters.SortBy?.ToLower() switch
        {
            "rating" => parameters.SortDirection == "desc" ? query.OrderByDescending(r => r.Rating) : query.OrderBy(r => r.Rating),
            _ => query.OrderByDescending(r => r.CreatedAt)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<ReviewDto>>(items);
        var result = new PagedResult<ReviewDto>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<ReviewDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<ReviewDto>> CreateAsync(ReviewCreateDto dto, int customerId)
    {
        var entity = _mapper.Map<Review>(dto);
        entity.CustomerId = customerId;
        entity.IsApproved = false;

        await _unitOfWork.Reviews.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Review created: {ReviewId} for {EntityType} {EntityId}", entity.Id, entity.EntityType, entity.EntityId);

        var result = _mapper.Map<ReviewDto>(entity);
        return ApiResponse<ReviewDto>.SuccessResponse(result, "Review submitted successfully.");
    }

    public async Task<ApiResponse<ReviewDto>> ReplyAsync(ReviewReplyDto dto, Guid repliedBy)
    {
        var entity = await _unitOfWork.Reviews.GetByIdAsync(dto.ReviewId);
        if (entity == null)
            return ApiResponse<ReviewDto>.FailResponse("Review not found.");

        entity.Reply = dto.Reply;
        entity.RepliedAt = DateTime.UtcNow;
        entity.RepliedBy = repliedBy;
        _unitOfWork.Reviews.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<ReviewDto>(entity);
        return ApiResponse<ReviewDto>.SuccessResponse(result, "Reply added successfully.");
    }

    public async Task<ApiResponse> ApproveAsync(int id)
    {
        var entity = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse.FailResponse("Review not found.");

        entity.IsApproved = true;
        _unitOfWork.Reviews.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Review approved successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Reviews.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse.FailResponse("Review not found.");

        _unitOfWork.Reviews.Remove(entity);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Review deleted successfully.");
    }

    public async Task<ApiResponse<IReadOnlyList<ReviewDto>>> GetByEntityAsync(string entityType, int entityId)
    {
        var reviews = await _unitOfWork.Reviews.GetReviewsByEntityAsync(entityType, entityId);
        var dtos = _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
        return ApiResponse<IReadOnlyList<ReviewDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<ReviewDto>>> GetByCustomerAsync(int customerId)
    {
        var reviews = await _unitOfWork.Reviews.GetReviewsByCustomerAsync(customerId);
        var dtos = _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
        return ApiResponse<IReadOnlyList<ReviewDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<ReviewDto>>> GetPendingAsync()
    {
        var reviews = await _unitOfWork.Reviews.GetPendingReviewsAsync();
        var dtos = _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
        return ApiResponse<IReadOnlyList<ReviewDto>>.SuccessResponse(dtos);
    }
}
