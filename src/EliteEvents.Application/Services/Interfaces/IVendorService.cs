using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Vendor;

namespace EliteEvents.Application.Services.Interfaces;

public interface IVendorService
{
    Task<ApiResponse<VendorDto>> GetByIdAsync(int id);
    Task<ApiResponse<PagedResult<VendorListDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<VendorDto>> CreateAsync(VendorCreateDto dto, Guid userId);
    Task<ApiResponse<VendorDto>> UpdateAsync(int id, VendorUpdateDto dto);
    Task<ApiResponse> DeleteAsync(int id);
    Task<ApiResponse<IReadOnlyList<VendorListDto>>> GetByCategoryAsync(int categoryId);
    Task<ApiResponse<IReadOnlyList<VendorListDto>>> GetTopRatedAsync(int count = 10);
    Task<ApiResponse<IReadOnlyList<VendorCategoryDto>>> GetCategoriesAsync();
    Task<ApiResponse<VendorCategoryDto>> CreateCategoryAsync(VendorCategoryDto dto);
}
