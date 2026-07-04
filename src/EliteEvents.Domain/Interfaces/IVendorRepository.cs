using EliteEvents.Domain.Entities;

namespace EliteEvents.Domain.Interfaces;

/// <summary>
/// Repository interface for Vendor-specific operations.
/// </summary>
public interface IVendorRepository : IRepository<Vendor>
{
    Task<IReadOnlyList<Vendor>> GetActiveVendorsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vendor>> GetVendorsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<Vendor?> GetVendorByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vendor>> GetTopRatedVendorsAsync(int count = 10, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vendor>> SearchVendorsAsync(string searchTerm, CancellationToken cancellationToken = default);
}
