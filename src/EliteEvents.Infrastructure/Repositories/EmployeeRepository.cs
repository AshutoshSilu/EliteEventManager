using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// Employee repository with specialized query methods.
/// </summary>
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context) { }

    public async Task<Employee?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
    }

    public async Task<bool> EmployeeCodeExistsAsync(string employeeCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.EmployeeCode == employeeCode, cancellationToken);
    }

    public IQueryable<Employee> QueryWithUser()
    {
        return _dbSet
            .Include(e => e.User)
            .ThenInclude(u => u.Role)
            .AsQueryable();
    }
}
