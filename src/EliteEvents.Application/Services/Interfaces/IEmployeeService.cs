using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Employee;

namespace EliteEvents.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<ApiResponse<EmployeeListItemDto>> RegisterAsync(EmployeeRegistrationDto dto);
    Task<ApiResponse<PagedResult<EmployeeListItemDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<EmployeeDetailDto>> GetByUserIdAsync(Guid userId);
    Task<ApiResponse<EmployeeDetailDto>> UpdateAsync(Guid userId, EmployeeUpdateDto dto);
    Task<ApiResponse<EmployeeListItemDto>> OnboardAsync(Guid userId);
    Task<ApiResponse<EmployeeListItemDto>> ResignAsync(Guid userId);
    Task<ApiResponse<EmployeeListItemDto>> TerminateAsync(Guid userId);
}
