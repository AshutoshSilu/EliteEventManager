using EliteEvents.Application.Services.Interfaces;

namespace EliteEvents.Infrastructure.Identity;

/// <summary>
/// BCrypt-based password hashing implementation.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Hashes a password using BCrypt with a work factor of 11.
    /// </summary>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(11));
    }

    /// <summary>
    /// Verifies a password against a BCrypt hash.
    /// </summary>
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
