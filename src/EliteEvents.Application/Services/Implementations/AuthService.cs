using AutoMapper;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Auth;
using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Entities;
using EliteEvents.Domain.Enums;
using EliteEvents.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EliteEvents.Application.Services.Implementations;

/// <summary>
/// Handles authentication operations including registration, login, and token management.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;

    public AuthService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
    }

    public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterDto dto)
    {
        // Check if email already exists
        if (await _unitOfWork.Users.EmailExistsAsync(dto.Email))
        {
            return ApiResponse<RegisterResponseDto>.FailResponse("Email address is already registered.");
        }

        // Create user entity
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email.ToLower(),
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            PhoneNumber = dto.PhoneNumber,
            RoleId = (int)UserRole.Customer,
            IsActive = true,
            IsEmailVerified = true, // Auto-verify for development
            EmailVerificationToken = Guid.NewGuid().ToString()
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Create customer profile
        var customer = new Customer { UserId = user.Id };
        // Note: Customer repository would be used here in full implementation

        // Send verification email
        await _emailService.SendEmailVerificationAsync(user.Email, user.EmailVerificationToken);

        var response = new RegisterResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Message = "Registration successful. Please verify your email."
        };

        return ApiResponse<RegisterResponseDto>.SuccessResponse(response, "Registration successful.");
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email.ToLower());

        if (user == null || !_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return ApiResponse<LoginResponseDto>.FailResponse("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            return ApiResponse<LoginResponseDto>.FailResponse("Your account has been deactivated.");
        }

        if (!user.IsEmailVerified)
        {
            return ApiResponse<LoginResponseDto>.FailResponse("Please verify your email before logging in.");
        }

        // Generate tokens
        var token = _jwtTokenService.GenerateToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Update user with refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        var response = new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.Name,
            Token = token,
            RefreshToken = refreshToken,
            TokenExpiry = DateTime.UtcNow.AddHours(1),
            ProfileImageUrl = user.ProfileImageUrl
        };

        return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful.");
    }

    public async Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var user = await _unitOfWork.Users.GetByRefreshTokenAsync(dto.RefreshToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return ApiResponse<LoginResponseDto>.FailResponse("Invalid or expired refresh token.");
        }

        var newToken = _jwtTokenService.GenerateToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        var response = new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.Name,
            Token = newToken,
            RefreshToken = newRefreshToken,
            TokenExpiry = DateTime.UtcNow.AddHours(1),
            ProfileImageUrl = user.ProfileImageUrl
        };

        return ApiResponse<LoginResponseDto>.SuccessResponse(response);
    }

    public async Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email.ToLower());
        if (user == null)
        {
            // Don't reveal that email doesn't exist
            return ApiResponse.SuccessResponse("If the email exists, a reset link has been sent.");
        }

        var rawToken = GenerateRawResetToken();
        user.PasswordResetToken = HashToken(rawToken);
        user.PasswordResetExpiry = DateTime.UtcNow.AddHours(1);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendPasswordResetAsync(user.Email, rawToken);

        return ApiResponse.SuccessResponse("If the email exists, a reset link has been sent.");
    }

    public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto dto)
    {
        if (dto.NewPassword != dto.ConfirmPassword)
        {
            return ApiResponse.FailResponse("Passwords do not match.");
        }

        var hashedToken = HashToken(dto.Token);
        var user = await _unitOfWork.Users.GetByPasswordResetTokenAsync(hashedToken);

        if (user == null || user.PasswordResetExpiry < DateTime.UtcNow)
        {
            return ApiResponse.FailResponse("Invalid or expired reset token.");
        }

        user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetExpiry = null;
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Password has been reset successfully.");
    }

    private static string GenerateRawResetToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static string HashToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }

    public async Task<ApiResponse> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return ApiResponse.FailResponse("User not found.");
        }

        if (!_passwordHasher.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
        {
            return ApiResponse.FailResponse("Current password is incorrect.");
        }

        user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Password changed successfully.");
    }

    public async Task<ApiResponse> VerifyEmailAsync(string token)
    {
        var user = await _unitOfWork.Users.GetByVerificationTokenAsync(token);
        if (user == null)
        {
            return ApiResponse.FailResponse("Invalid verification token.");
        }

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Email verified successfully.");
    }

    public async Task<ApiResponse> LogoutAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return ApiResponse.FailResponse("User not found.");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResponse("Logged out successfully.");
    }
}
