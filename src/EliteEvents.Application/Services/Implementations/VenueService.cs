using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Venue;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

public class VenueService : IVenueService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<VenueService> _logger;

    public VenueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VenueService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<VenueDto>> GetByIdAsync(int id)
    {
        var venue = await _unitOfWork.Venues.GetVenueWithImagesAsync(id);
        if (venue == null)
            return ApiResponse<VenueDto>.FailResponse("Venue not found.");

        var dto = _mapper.Map<VenueDto>(venue);
        return ApiResponse<VenueDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResult<VenueListDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Venues.Query().Where(v => v.IsActive);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.ToLower();
            query = query.Where(v => v.Name.ToLower().Contains(search) ||
                                     v.Address.ToLower().Contains(search));
        }

        query = parameters.SortBy?.ToLower() switch
        {
            "name" => parameters.SortDirection == "desc" ? query.OrderByDescending(v => v.Name) : query.OrderBy(v => v.Name),
            "capacity" => parameters.SortDirection == "desc" ? query.OrderByDescending(v => v.Capacity) : query.OrderBy(v => v.Capacity),
            "price" => parameters.SortDirection == "desc" ? query.OrderByDescending(v => v.PricePerDay) : query.OrderBy(v => v.PricePerDay),
            _ => query.OrderBy(v => v.Name)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<VenueListDto>>(items);
        var result = new PagedResult<VenueListDto>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<VenueListDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<VenueDto>> CreateAsync(VenueCreateDto dto)
    {
        var entity = _mapper.Map<Venue>(dto);
        entity.IsActive = true;

        await _unitOfWork.Venues.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Venue created: {VenueId} - {Name}", entity.Id, entity.Name);

        var result = _mapper.Map<VenueDto>(entity);
        return ApiResponse<VenueDto>.SuccessResponse(result, "Venue created successfully.");
    }

    public async Task<ApiResponse<VenueDto>> UpdateAsync(int id, VenueUpdateDto dto)
    {
        var entity = await _unitOfWork.Venues.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse<VenueDto>.FailResponse("Venue not found.");

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Venues.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<VenueDto>(entity);
        return ApiResponse<VenueDto>.SuccessResponse(result, "Venue updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Venues.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse.FailResponse("Venue not found.");

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Venues.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Venue deleted successfully.");
    }

    public async Task<ApiResponse<IReadOnlyList<VenueListDto>>> GetFeaturedAsync(int count = 6)
    {
        var venues = await _unitOfWork.Venues.GetFeaturedVenuesAsync(count);
        var dtos = _mapper.Map<IReadOnlyList<VenueListDto>>(venues);
        return ApiResponse<IReadOnlyList<VenueListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<PagedResult<VenueListDto>>> SearchAsync(string searchTerm, int? minCapacity, int? maxCapacity, QueryParameters parameters)
    {
        var venues = await _unitOfWork.Venues.SearchVenuesAsync(searchTerm, minCapacity, maxCapacity);
        var dtos = _mapper.Map<IReadOnlyList<VenueListDto>>(venues);
        var result = new PagedResult<VenueListDto>(dtos, dtos.Count, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<VenueListDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<bool>> CheckAvailabilityAsync(int venueId, DateOnly date, string? startTime, string? endTime)
    {
        TimeOnly? start = startTime != null ? TimeOnly.Parse(startTime) : null;
        TimeOnly? end = endTime != null ? TimeOnly.Parse(endTime) : null;

        var isAvailable = await _unitOfWork.Venues.IsVenueAvailableAsync(venueId, date, start, end);
        return ApiResponse<bool>.SuccessResponse(isAvailable);
    }

    public async Task<ApiResponse<IReadOnlyList<VenueAvailabilityDto>>> GetAvailabilityAsync(int venueId, DateOnly startDate, DateOnly endDate)
    {
        var availabilities = new List<VenueAvailabilityDto>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var isAvailable = await _unitOfWork.Venues.IsVenueAvailableAsync(venueId, date);
            availabilities.Add(new VenueAvailabilityDto
            {
                Date = date,
                IsAvailable = isAvailable
            });
        }
        return ApiResponse<IReadOnlyList<VenueAvailabilityDto>>.SuccessResponse(availabilities);
    }
}
