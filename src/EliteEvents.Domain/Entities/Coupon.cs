using EliteEvents.Domain.Common;

namespace EliteEvents.Domain.Entities;

/// <summary>
/// Represents a discount coupon.
/// </summary>
public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DiscountType { get; set; } = "Percentage"; // Percentage, FixedAmount
    public decimal DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Computed
    public bool IsValid => IsActive && DateOnly.FromDateTime(DateTime.UtcNow) >= StartDate
                           && DateOnly.FromDateTime(DateTime.UtcNow) <= EndDate
                           && (!UsageLimit.HasValue || UsedCount < UsageLimit.Value);
}
