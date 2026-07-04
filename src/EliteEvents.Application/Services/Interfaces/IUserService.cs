using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.User;

namespace EliteEvents.Application.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponse<UserDto>> GetByIdAsync(Guid id);
    Task<ApiResponse<PagedResult<UserDto>>> GetAllAsync(QueryParameters parameters);
    Task<ApiResponse<UserDto>> CreateAsync(UserCreateDto dto);
    Task<ApiResponse<UserDto>> UpdateAsync(Guid id, UserUpdateDto dto);
    Task<ApiResponse> DeleteAsync(Guid id);
    Task<ApiResponse<UserProfileDto>> GetProfileAsync(Guid userId);
    Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
    Task<ApiResponse<IReadOnlyList<UserDto>>> GetByRoleAsync(int roleId);
}
