using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.User;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return ApiResponse<UserDto>.FailResponse("User not found.");

        var dto = _mapper.Map<UserDto>(user);
        return ApiResponse<UserDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<PagedResult<UserDto>>> GetAllAsync(QueryParameters parameters)
    {
        var query = _unitOfWork.Users.Query();

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.ToLower();
            query = query.Where(u => u.FirstName.ToLower().Contains(search) ||
                                     u.LastName.ToLower().Contains(search) ||
                                     u.Email.ToLower().Contains(search));
        }

        query = parameters.SortBy?.ToLower() switch
        {
            "name" => parameters.SortDirection == "desc" ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
            "email" => parameters.SortDirection == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            _ => query.OrderByDescending(u => u.CreatedAt)
        };

        var totalCount = query.Count();
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<UserDto>>(items);
        var result = new PagedResult<UserDto>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<UserDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<UserDto>> CreateAsync(UserCreateDto dto)
    {
        if (await _unitOfWork.Users.EmailExistsAsync(dto.Email))
            return ApiResponse<UserDto>.FailResponse("Email already exists.");

        var entity = _mapper.Map<User>(dto);
        entity.PasswordHash = _passwordHasher.HashPassword(dto.Password);
        entity.IsActive = dto.IsActive;

        await _unitOfWork.Users.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<UserDto>(entity);
        return ApiResponse<UserDto>.SuccessResponse(result, "User created successfully.");
    }

    public async Task<ApiResponse<UserDto>> UpdateAsync(Guid id, UserUpdateDto dto)
    {
        var entity = await _unitOfWork.Users.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse<UserDto>.FailResponse("User not found.");

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<UserDto>(entity);
        return ApiResponse<UserDto>.SuccessResponse(result, "User updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Users.GetByIdAsync(id);
        if (entity == null)
            return ApiResponse.FailResponse("User not found.");

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("User deleted successfully.");
    }

    public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return ApiResponse<UserProfileDto>.FailResponse("User not found.");

        var dto = _mapper.Map<UserProfileDto>(user);
        return ApiResponse<UserProfileDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(Guid userId, UpdateProfileDto dto)
    {
        var entity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (entity == null)
            return ApiResponse<UserProfileDto>.FailResponse("User not found.");

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<UserProfileDto>(entity);
        return ApiResponse<UserProfileDto>.SuccessResponse(result, "Profile updated successfully.");
    }

    public async Task<ApiResponse<IReadOnlyList<UserDto>>> GetByRoleAsync(int roleId)
    {
        var users = await _unitOfWork.Users.GetUsersByRoleAsync(roleId);
        var dtos = _mapper.Map<IReadOnlyList<UserDto>>(users);
        return ApiResponse<IReadOnlyList<UserDto>>.SuccessResponse(dtos);
    }
}
