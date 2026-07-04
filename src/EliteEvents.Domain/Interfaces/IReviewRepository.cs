using EliteEvents.Domain.Entities;

namespace EliteEvents.Domain.Interfaces;

/// <summary>
/// Repository interface for Review-specific operations.
/// </summary>
public interface IReviewRepository : IRepository<Review>
{
    Task<IReadOnlyList<Review>> GetReviewsByEntityAsync(string entityType, int entityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetReviewsByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingAsync(string entityType, int entityId, CancellationToken cancellationToken = default);
    Task<bool> HasCustomerReviewedAsync(int customerId, string entityType, int entityId, CancellationToken cancellationToken = default);
}
