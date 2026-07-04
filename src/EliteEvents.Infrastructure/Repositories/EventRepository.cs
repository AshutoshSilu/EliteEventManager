using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Event repository with specialized query methods.
/// </summary>
public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Event>> GetPublishedEventsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Where(e => e.IsPublished && !e.IsDeleted)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetFeaturedEventsAsync(int count = 6, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Where(e => e.IsFeatured && e.IsPublished && !e.IsDeleted && e.StartDate > DateTime.UtcNow)
            .OrderBy(e => e.StartDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetUpcomingEventsAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Where(e => e.IsPublished && !e.IsDeleted && e.StartDate > DateTime.UtcNow)
            .OrderBy(e => e.StartDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetEventsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Where(e => e.CategoryId == categoryId && e.IsPublished && !e.IsDeleted)
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetEventWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Include(e => e.Images)
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> SearchEventsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var term = searchTerm.ToLower();
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Venue)
            .Where(e => !e.IsDeleted && e.IsPublished &&
                (e.Title.ToLower().Contains(term) ||
                 (e.Description != null && e.Description.ToLower().Contains(term)) ||
                 e.Category.Name.ToLower().Contains(term) ||
                 (e.Tags != null && e.Tags.ToLower().Contains(term))))
            .OrderBy(e => e.StartDate)
            .ToListAsync(cancellationToken);
    }
}
