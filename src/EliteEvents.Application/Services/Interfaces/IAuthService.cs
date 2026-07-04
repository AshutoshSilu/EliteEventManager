using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Auth;

namespace EliteEvents.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto);
    Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
    Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto dto);
    Task<ApiResponse> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    Task<ApiResponse> VerifyEmailAsync(string token);
    Task<ApiResponse> LogoutAsync(Guid userId);
}
