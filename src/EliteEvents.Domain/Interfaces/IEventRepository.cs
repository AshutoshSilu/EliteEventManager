using EliteEvents.Domain.Entities;

namespace EliteEvents.Domain.Interfaces;

/// <summary>
/// Repository interface for Event-specific operations.
/// </summary>
public interface IEventRepository : IRepository<Event>
{
    Task<IReadOnlyList<Event>> GetPublishedEventsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetFeaturedEventsAsync(int count = 6, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetUpcomingEventsAsync(int count = 10, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetEventsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<Event?> GetEventWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> SearchEventsAsync(string searchTerm, CancellationToken cancellationToken = default);
}
