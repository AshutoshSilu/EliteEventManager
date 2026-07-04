using EliteEvents.Domain.Entities;

namespace EliteEvents.Domain.Interfaces;

/// <summary>
/// Repository interface for User-specific operations.
/// </summary>
public interface IUserRepository : IRepositoryGuid<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<User?> GetByVerificationTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> GetUsersByRoleAsync(int roleId, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
