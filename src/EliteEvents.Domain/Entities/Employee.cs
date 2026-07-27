using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents an employee profile linked to a user account.
/// </summary>
public class Employee : AuditableEntity
{
    public Guid UserId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public DateOnly DateOfJoining { get; set; }
    public string EmploymentStatus { get; set; } = "Pending Onboarding";
    public decimal? Salary { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual City? City { get; set; }
}
