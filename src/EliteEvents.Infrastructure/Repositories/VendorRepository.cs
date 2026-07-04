using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Vendor repository with specialized query methods.
/// </summary>
public class VendorRepository : Repository<Vendor>, IVendorRepository
{
    public VendorRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Vendor>> GetActiveVendorsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.Category)
            .Include(v => v.City)
            .Where(v => v.IsActive)
            .OrderBy(v => v.BusinessName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Vendor>> GetVendorsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.Category)
            .Where(v => v.CategoryId == categoryId && v.IsActive)
            .OrderByDescending(v => v.Rating)
            .ToListAsync(cancellationToken);
    }

    public async Task<Vendor?> GetVendorByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.Category)
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.UserId == userId, cancellationToken);
    }

    public async Task<IReadOnlyList<Vendor>> GetTopRatedVendorsAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.Category)
            .Where(v => v.IsActive && v.IsVerified)
            .OrderByDescending(v => v.Rating)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Vendor>> SearchVendorsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var term = searchTerm.ToLower();
        return await _dbSet
            .Include(v => v.Category)
            .Where(v => v.IsActive &&
                (v.BusinessName.ToLower().Contains(term) ||
                 (v.Description != null && v.Description.ToLower().Contains(term)) ||
                 v.Category.Name.ToLower().Contains(term)))
            .OrderByDescending(v => v.Rating)
            .ToListAsync(cancellationToken);
    }
}
