namespace EliteEvents.Application.DTOs.Employee;

public static class EmployeeStatuses
{
    public const string PendingOnboarding = "Pending Onboarding";
    public const string Onboarded = "Onboarded";
    public const string Resigned = "Resigned";
    public const string Terminated = "Terminated";

    public static readonly string[] All =
    {
        PendingOnboarding,
        Onboarded,
        Resigned,
        Terminated
    };
}

public class EmployeeRegistrationDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public DateTime JoiningDate { get; set; }
    public string ProfilePhotoDataUrl { get; set; } = string.Empty;
}

public class EmployeeListItemDto
{
    public Guid UserId { get; set; }
    public string? EmployeeCode { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? MobileNumber { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public string? Address { get; set; }
    public DateTime JoiningDate { get; set; }
    public string CurrentStatus { get; set; } = EmployeeStatuses.PendingOnboarding;
    public string? ProfilePhotoUrl { get; set; }
    public bool IsActive { get; set; }
}

public class EmployeeDetailDto : EmployeeListItemDto
{
}

public class EmployeeUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public DateTime JoiningDate { get; set; }
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public string? Address { get; set; }
}
