using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Employee;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteEvents.Application.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IFileStorageService _fileStorageService;
    private readonly IEmailService _emailService;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IFileStorageService fileStorageService,
        IEmailService emailService,
        ILogger<EmployeeService> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _fileStorageService = fileStorageService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<ApiResponse<EmployeeListItemDto>> RegisterAsync(EmployeeRegistrationDto dto)
    {
        if (await _unitOfWork.Users.EmailExistsAsync(dto.Email.ToLower()))
            return ApiResponse<EmployeeListItemDto>.FailResponse("Email already exists.");

        if (dto.JoiningDate == default)
            return ApiResponse<EmployeeListItemDto>.FailResponse("Joining date is required.");

        try
        {
            var imagePath = await SaveProfilePhotoAsync(dto.ProfilePhotoDataUrl);
            var employeeCode = await GenerateEmployeeCodeAsync();

            var user = new User
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim().ToLower(),
                PasswordHash = _passwordHasher.HashPassword(GenerateTemporaryPassword()),
                PhoneNumber = dto.MobileNumber.Trim(),
                RoleId = dto.RoleId,
                ProfileImageUrl = imagePath,
                IsActive = true,
                IsEmailVerified = true
            };

            await _unitOfWork.Users.AddAsync(user);

            var employee = new Employee
            {
                UserId = user.Id,
                EmployeeCode = employeeCode,
                DateOfJoining = DateOnly.FromDateTime(dto.JoiningDate),
                EmploymentStatus = EmployeeStatuses.PendingOnboarding,
                IsActive = true
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.Employees.GetByUserIdAsync(user.Id);
            var result = MapToListItemDto(created!);
            return ApiResponse<EmployeeListItemDto>.SuccessResponse(result, "Employee registered successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Employee registration failed for {Email}", dto.Email);
            return ApiResponse<EmployeeListItemDto>.FailResponse("Unable to register employee.");
        }
    }

    public async Task<ApiResponse<PagedResult<EmployeeListItemDto>>> GetAllAsync(QueryParameters parameters)
    {
        var employees = _unitOfWork.Employees.QueryWithUser()
            .Where(e => !e.User.IsDeleted)
            .ToList()
            .Select(MapToListItemDto)
            .ToList();

        // Include administrator users even if they do not have an employee profile.
        var admins = await _unitOfWork.Users.GetUsersByRoleAsync(1);
        foreach (var admin in admins)
        {
            if (employees.Any(e => e.UserId == admin.Id))
                continue;

            employees.Add(MapUserToListItemDto(admin));
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var search = parameters.SearchTerm.ToLower();
            employees = employees
                .Where(e => e.FirstName.ToLower().Contains(search) ||
                            e.LastName.ToLower().Contains(search) ||
                            e.Email.ToLower().Contains(search))
                .ToList();
        }

        employees = parameters.SortBy?.ToLower() switch
        {
            "name" => parameters.SortDirection == "desc"
                ? employees.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName).ToList()
                : employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList(),
            "joiningdate" => parameters.SortDirection == "desc"
                ? employees.OrderByDescending(e => e.JoiningDate).ToList()
                : employees.OrderBy(e => e.JoiningDate).ToList(),
            _ => employees.OrderByDescending(e => e.JoiningDate).ToList()
        };

        var totalCount = employees.Count;
        var items = employees
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        var result = new PagedResult<EmployeeListItemDto>(items, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<EmployeeListItemDto>>.SuccessResponse(result);
    }

    public async Task<ApiResponse<EmployeeDetailDto>> GetByUserIdAsync(Guid userId)
    {
        var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
        if (employee == null)
            return ApiResponse<EmployeeDetailDto>.FailResponse("Employee not found.");

        return ApiResponse<EmployeeDetailDto>.SuccessResponse(MapToDetailDto(employee));
    }

    public async Task<ApiResponse<EmployeeDetailDto>> UpdateAsync(Guid userId, EmployeeUpdateDto dto)
    {
        var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
        if (employee == null)
            return ApiResponse<EmployeeDetailDto>.FailResponse("Employee not found.");

        var normalizedEmail = dto.Email.Trim().ToLower();
        if (!string.Equals(employee.User.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase) &&
            await _unitOfWork.Users.EmailExistsAsync(normalizedEmail))
        {
            return ApiResponse<EmployeeDetailDto>.FailResponse("Email already exists.");
        }

        employee.User.FirstName = dto.FirstName.Trim();
        employee.User.LastName = dto.LastName.Trim();
        employee.User.Email = normalizedEmail;
        employee.User.PhoneNumber = dto.MobileNumber.Trim();
        employee.User.RoleId = dto.RoleId;
        employee.User.UpdatedAt = DateTime.UtcNow;

        employee.DateOfJoining = DateOnly.FromDateTime(dto.JoiningDate);
        employee.Department = string.IsNullOrWhiteSpace(dto.Department) ? null : dto.Department.Trim();
        employee.Designation = string.IsNullOrWhiteSpace(dto.Designation) ? null : dto.Designation.Trim();
        employee.Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim();
        employee.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(employee.User);
        _unitOfWork.Employees.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<EmployeeDetailDto>.SuccessResponse(MapToDetailDto(employee), "Employee details updated successfully.");
    }

    public async Task<ApiResponse<EmployeeListItemDto>> OnboardAsync(Guid userId)
    {
        var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
        if (employee == null)
            return ApiResponse<EmployeeListItemDto>.FailResponse("Employee not found.");

        if (employee.EmploymentStatus != EmployeeStatuses.PendingOnboarding)
            return ApiResponse<EmployeeListItemDto>.FailResponse("Only pending employees can be onboarded.");

        employee.EmploymentStatus = EmployeeStatuses.Onboarded;
        employee.IsActive = true;
        employee.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Employees.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendWelcomeLetterAsync(
            employee.User.Email,
            employee.User.FullName,
            employee.EmployeeCode,
            ResolvePosition(employee),
            employee.User.PhoneNumber);

        return ApiResponse<EmployeeListItemDto>.SuccessResponse(MapToListItemDto(employee), "Employee onboarded successfully.");
    }

    public async Task<ApiResponse<EmployeeListItemDto>> ResignAsync(Guid userId)
    {
        var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
        if (employee == null)
            return ApiResponse<EmployeeListItemDto>.FailResponse("Employee not found.");

        if (employee.EmploymentStatus != EmployeeStatuses.Onboarded)
            return ApiResponse<EmployeeListItemDto>.FailResponse("Only onboarded employees can resign.");

        employee.EmploymentStatus = EmployeeStatuses.Resigned;
        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;
        employee.User.IsActive = false;

        _unitOfWork.Employees.Update(employee);
        _unitOfWork.Users.Update(employee.User);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<EmployeeListItemDto>.SuccessResponse(MapToListItemDto(employee), "Employee marked as resigned.");
    }

    public async Task<ApiResponse<EmployeeListItemDto>> TerminateAsync(Guid userId)
    {
        var employee = await _unitOfWork.Employees.GetByUserIdAsync(userId);
        if (employee == null)
            return ApiResponse<EmployeeListItemDto>.FailResponse("Employee not found.");

        if (employee.EmploymentStatus == EmployeeStatuses.Resigned || employee.EmploymentStatus == EmployeeStatuses.Terminated)
            return ApiResponse<EmployeeListItemDto>.FailResponse("Employee is already inactive.");

        employee.EmploymentStatus = EmployeeStatuses.Terminated;
        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;
        employee.User.IsActive = false;

        _unitOfWork.Employees.Update(employee);
        _unitOfWork.Users.Update(employee.User);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<EmployeeListItemDto>.SuccessResponse(MapToListItemDto(employee), "Employee terminated successfully.");
    }

    private async Task<string?> SaveProfilePhotoAsync(string dataUrl)
    {
        if (string.IsNullOrWhiteSpace(dataUrl))
            return null;

        if (!dataUrl.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
            return dataUrl;

        var commaIndex = dataUrl.IndexOf(',');
        if (commaIndex < 0)
            return null;

        var metadata = dataUrl[..commaIndex];
        var base64Part = dataUrl[(commaIndex + 1)..];

        var mime = metadata.Split(';')[0].Replace("data:", "", StringComparison.OrdinalIgnoreCase);
        var extension = mime switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => ".jpg"
        };

        var bytes = Convert.FromBase64String(base64Part);
        await using var stream = new MemoryStream(bytes);
        return await _fileStorageService.UploadFileAsync(stream, $"employee{extension}", "employees");
    }

    private async Task<string> GenerateEmployeeCodeAsync()
    {
        for (var i = 0; i < 5; i++)
        {
            var candidate = $"E{DateTime.UtcNow:yyMMdd}{Random.Shared.Next(1000, 9999)}";
            if (!await _unitOfWork.Employees.EmployeeCodeExistsAsync(candidate))
            {
                return candidate;
            }
        }

        return $"E{DateTime.UtcNow:yyMMddHHmm}";
    }

    private static EmployeeListItemDto MapToListItemDto(Employee employee)
    {
        return new EmployeeListItemDto
        {
            UserId = employee.UserId,
            EmployeeCode = employee.EmployeeCode,
            FirstName = employee.User.FirstName,
            LastName = employee.User.LastName,
            FullName = employee.User.FullName,
            Email = employee.User.Email,
            MobileNumber = employee.User.PhoneNumber,
            RoleId = employee.User.RoleId,
            RoleName = GetRoleName(employee.User.RoleId),
            Department = employee.Department,
            Designation = employee.Designation,
            Address = employee.Address,
            JoiningDate = employee.DateOfJoining.ToDateTime(TimeOnly.MinValue),
            CurrentStatus = employee.EmploymentStatus,
            ProfilePhotoUrl = employee.User.ProfileImageUrl,
            IsActive = employee.IsActive
        };
    }

    private static EmployeeDetailDto MapToDetailDto(Employee employee)
    {
        return new EmployeeDetailDto
        {
            UserId = employee.UserId,
            EmployeeCode = employee.EmployeeCode,
            FirstName = employee.User.FirstName,
            LastName = employee.User.LastName,
            FullName = employee.User.FullName,
            Email = employee.User.Email,
            MobileNumber = employee.User.PhoneNumber,
            RoleId = employee.User.RoleId,
            RoleName = GetRoleName(employee.User.RoleId),
            Department = employee.Department,
            Designation = employee.Designation,
            Address = employee.Address,
            JoiningDate = employee.DateOfJoining.ToDateTime(TimeOnly.MinValue),
            CurrentStatus = employee.EmploymentStatus,
            ProfilePhotoUrl = employee.User.ProfileImageUrl,
            IsActive = employee.IsActive
        };
    }

    private static EmployeeListItemDto MapUserToListItemDto(User user)
    {
        var joiningDate = user.Employee?.DateOfJoining.ToDateTime(TimeOnly.MinValue) ?? user.CreatedAt;
        var status = user.Employee?.EmploymentStatus;

        if (string.IsNullOrWhiteSpace(status))
        {
            status = user.Role.Name == "Administrator"
                ? EmployeeStatuses.Onboarded
                : EmployeeStatuses.PendingOnboarding;
        }

        return new EmployeeListItemDto
        {
            UserId = user.Id,
            EmployeeCode = user.Employee?.EmployeeCode,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Email = user.Email,
            MobileNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = GetRoleName(user.RoleId),
            Department = user.Employee?.Department,
            Designation = user.Employee?.Designation,
            Address = user.Employee?.Address,
            JoiningDate = joiningDate,
            CurrentStatus = status,
            ProfilePhotoUrl = user.ProfileImageUrl,
            IsActive = user.IsActive
        };
    }

    private static string ResolvePosition(Employee employee)
    {
        if (!string.IsNullOrWhiteSpace(employee.Designation))
            return employee.Designation;

        return GetRoleName(employee.User.RoleId);
    }

    private static string GetRoleName(int roleId)
    {
        return roleId switch
        {
            1 => "Administrator",
            2 => "EventManager",
            3 => "Customer",
            4 => "Vendor",
            5 => "Visitor",
            _ => "Unknown"
        };
    }

    private static string GenerateTemporaryPassword()
    {
        return $"Emp#{Guid.NewGuid().ToString("N")[..8]}A1";
    }
}
