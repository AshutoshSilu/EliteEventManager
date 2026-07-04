using EliteEvents.Domain.Entities;

namespace EliteEvents.Domain.Interfaces;

/// <summary>
/// Repository interface for Venue-specific operations.
/// </summary>
public interface IVenueRepository : IRepository<Venue>
{
    Task<IReadOnlyList<Venue>> GetActiveVenuesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Venue>> GetFeaturedVenuesAsync(int count = 6, CancellationToken cancellationToken = default);
    Task<Venue?> GetVenueWithImagesAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Venue>> GetVenuesByCityAsync(int cityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Venue>> SearchVenuesAsync(string searchTerm, int? minCapacity = null, int? maxCapacity = null, CancellationToken cancellationToken = default);
    Task<bool> IsVenueAvailableAsync(int venueId, DateOnly date, TimeOnly? startTime = null, TimeOnly? endTime = null, CancellationToken cancellationToken = default);
}
