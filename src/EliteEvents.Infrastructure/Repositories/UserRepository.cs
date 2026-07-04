using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Repositories;

/// <summary>
/// User repository with specialized query methods.
/// </summary>
public class UserRepository : RepositoryGuid<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && !u.IsDeleted, cancellationToken);
    }

    public async Task<User?> GetByVerificationTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == token && !u.IsDeleted, cancellationToken);
    }

    public async Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.PasswordResetToken == token && !u.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetUsersByRoleAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Role)
            .Where(u => u.RoleId == roleId && !u.IsDeleted)
            .OrderBy(u => u.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }
}
