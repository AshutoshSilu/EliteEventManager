using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Venue repository with specialized query methods.
/// </summary>
public class VenueRepository : Repository<Venue>, IVenueRepository
{
    public VenueRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Venue>> GetActiveVenuesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.City)
            .Where(v => v.IsActive && !v.IsDeleted)
            .OrderBy(v => v.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Venue>> GetFeaturedVenuesAsync(int count = 6, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.City)
            .Include(v => v.Images)
            .Where(v => v.IsFeatured && v.IsActive && !v.IsDeleted)
            .OrderByDescending(v => v.Rating)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<Venue?> GetVenueWithImagesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.City)
            .Include(v => v.Images.OrderBy(i => i.SortOrder))
            .Include(v => v.Availability)
            .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Venue>> GetVenuesByCityAsync(int cityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.City)
            .Where(v => v.CityId == cityId && v.IsActive && !v.IsDeleted)
            .OrderBy(v => v.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Venue>> SearchVenuesAsync(string searchTerm, int? minCapacity = null, int? maxCapacity = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(v => v.City)
            .Where(v => v.IsActive && !v.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(v => v.Name.ToLower().Contains(term) ||
                                     v.Address.ToLower().Contains(term) ||
                                     (v.Description != null && v.Description.ToLower().Contains(term)));
        }

        if (minCapacity.HasValue)
            query = query.Where(v => v.Capacity >= minCapacity.Value);

        if (maxCapacity.HasValue)
            query = query.Where(v => v.Capacity <= maxCapacity.Value);

        return await query.OrderBy(v => v.Name).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsVenueAvailableAsync(int venueId, DateOnly date, TimeOnly? startTime = null, TimeOnly? endTime = null, CancellationToken cancellationToken = default)
    {
        var hasBooking = await _context.Bookings
            .AnyAsync(b => b.VenueId == venueId &&
                          b.EventDate == date &&
                          b.Status != Domain.Enums.BookingStatus.Cancelled &&
                          b.Status != Domain.Enums.BookingStatus.Refunded, cancellationToken);

        if (hasBooking) return false;

        var availability = await _context.VenueAvailabilities
            .FirstOrDefaultAsync(a => a.VenueId == venueId && a.Date == date, cancellationToken);

        return availability == null || availability.IsAvailable;
    }
}
