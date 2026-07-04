using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a system user with authentication details.
/// </summary>
public class User : AuditableEntityGuid, ISoftDeletable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; }
    public string? EmailVerificationToken { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetExpiry { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsDeleted { get; set; }

    // Computed property
    public string FullName => $"{FirstName} {LastName}";

    // Navigation properties
    public virtual Role Role { get; set; } = null!;
    public virtual Customer? Customer { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual Vendor? Vendor { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
