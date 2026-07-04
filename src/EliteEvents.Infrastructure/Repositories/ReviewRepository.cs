using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Review repository with specialized query methods.
/// </summary>
public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Review>> GetReviewsByEntityAsync(string entityType, int entityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Customer).ThenInclude(c => c.User)
            .Where(r => r.EntityType == entityType && r.EntityId == entityId && r.IsActive && r.IsApproved)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetReviewsByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.CustomerId == customerId && r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetPendingReviewsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Customer).ThenInclude(c => c.User)
            .Where(r => !r.IsApproved && r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<double> GetAverageRatingAsync(string entityType, int entityId, CancellationToken cancellationToken = default)
    {
        var reviews = await _dbSet
            .Where(r => r.EntityType == entityType && r.EntityId == entityId && r.IsActive && r.IsApproved)
            .ToListAsync(cancellationToken);

        return reviews.Count > 0 ? reviews.Average(r => r.Rating) : 0;
    }

    public async Task<bool> HasCustomerReviewedAsync(int customerId, string entityType, int entityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(r =>
            r.CustomerId == customerId &&
            r.EntityType == entityType &&
            r.EntityId == entityId &&
            r.IsActive, cancellationToken);
    }
}
