using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Vendor;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

public class VendorService : IVendorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<VendorService> _logger;

    public VendorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VendorService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<VendorDto>> GetByIdAsync(int id)
    {
        var vendor = await _unitOfWork.Vendors.GetByIdAsync(id);
        if (vendor == null)
            return ApiResponse<VendorDto>.FailResponse("Vendor not found.");

        var dto = _mapper.Map<VendorDto>(vendor);
        return ApiResponse<VendorDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResult<VendorListDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Vendors.Query().Where(v => v.IsActive);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.ToLower();
            query = query.Where(v => v.BusinessName.ToLower().Contains(search));
        }

        query = parameters.SortBy?.ToLower() switch
        {
            "name" => parameters.SortDirection == "desc" ? query.OrderByDescending(v => v.BusinessName) : query.OrderBy(v => v.BusinessName),
            "rating" => parameters.SortDirection == "desc" ? query.OrderByDescending(v => v.Rating) : query.OrderBy(v => v.Rating),
            _ => query.OrderBy(v => v.BusinessName)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<VendorListDto>>(items);
        var result = new PagedResult<VendorListDto>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<VendorListDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<VendorDto>> CreateAsync(VendorCreateDto dto, Guid userId)
    {
        var entity = _mapper.Map<Vendor>(dto);
        entity.UserId = userId;
        entity.IsActive = true;

        await _unitOfWork.Vendors.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Vendor created: {VendorId} - {Name}", entity.Id, entity.BusinessName);

        var result = _mapper.Map<VendorDto>(entity);
        return ApiResponse<VendorDto>.SuccessResponse(result, "Vendor created successfully.");
    }

    public async Task<ApiResponse<VendorDto>> UpdateAsync(int id, VendorUpdateDto dto)
    {
        var entity = await _unitOfWork.Vendors.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse<VendorDto>.FailResponse("Vendor not found.");

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Vendors.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<VendorDto>(entity);
        return ApiResponse<VendorDto>.SuccessResponse(result, "Vendor updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.Vendors.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse.FailResponse("Vendor not found.");

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Vendors.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Vendor deleted successfully.");
    }

    public async Task<ApiResponse<IReadOnlyList<VendorListDto>>> GetByCategoryAsync(int categoryId)
    {
        var vendors = await _unitOfWork.Vendors.GetVendorsByCategoryAsync(categoryId);
        var dtos = _mapper.Map<IReadOnlyList<VendorListDto>>(vendors);
        return ApiResponse<IReadOnlyList<VendorListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<VendorListDto>>> GetTopRatedAsync(int count = 10)
    {
        var vendors = await _unitOfWork.Vendors.GetTopRatedVendorsAsync(count);
        var dtos = _mapper.Map<IReadOnlyList<VendorListDto>>(vendors);
        return ApiResponse<IReadOnlyList<VendorListDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<VendorCategoryDto>>> GetCategoriesAsync()
    {
        // Return empty list - in production, use a dedicated category repository
        return ApiResponse<IReadOnlyList<VendorCategoryDto>>.SuccessResponse(new List<VendorCategoryDto>());
    }

    public async Task<ApiResponse<VendorCategoryDto>> CreateCategoryAsync(VendorCategoryDto dto)
    {
        return ApiResponse<VendorCategoryDto>.SuccessResponse(dto, "Category created successfully.");
    }
}
