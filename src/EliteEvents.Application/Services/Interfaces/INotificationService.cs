using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Notification;

namespace EliteEvents.Application.Services.Interfaces;

public interface INotificationService
{
    Task<ApiResponse<IReadOnlyList<NotificationDto>>> GetUserNotificationsAsync(Guid userId, bool unreadOnly = false);
    Task<ApiResponse<int>> GetUnreadCountAsync(Guid userId);
    Task<ApiResponse> MarkAsReadAsync(int notificationId, Guid userId);
    Task<ApiResponse> MarkAllAsReadAsync(Guid userId);
    Task<ApiResponse> CreateAsync(NotificationCreateDto dto);
    Task<ApiResponse> CreateBulkAsync(NotificationBulkCreateDto dto);
    Task<ApiResponse> DeleteAsync(int id, Guid userId);
}
