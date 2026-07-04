using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation managing transactions across repositories.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private IUserRepository? _users;
    private IEventRepository? _events;
    private IBookingRepository? _bookings;
    private IVenueRepository? _venues;
    private IVendorRepository? _vendors;
    private IPaymentRepository? _payments;
    private IReviewRepository? _reviews;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IEventRepository Events => _events ??= new EventRepository(_context);
    public IBookingRepository Bookings => _bookings ??= new BookingRepository(_context);
    public IVenueRepository Venues => _venues ??= new VenueRepository(_context);
    public IVendorRepository Vendors => _vendors ??= new VendorRepository(_context);
    public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);
    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
