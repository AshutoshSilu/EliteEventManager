using System.Security.Claims;
using EliteEvents.Application.DTOs.Notification;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Notification management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Get current user's notifications.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications([FromQuery] bool unreadOnly = false)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _notificationService.GetUserNotificationsAsync(userId, unreadOnly);
        return Ok(result);
    }

    /// <summary>
    /// Get unread notification count.
    /// </summary>
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _notificationService.GetUnreadCountAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Mark a notification as read.
    /// </summary>
    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _notificationService.MarkAsReadAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Mark all notifications as read.
    /// </summary>
    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _notificationService.MarkAllAsReadAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Send a notification. Admin/Manager only.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationCreateDto dto)
    {
        var result = await _notificationService.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Send bulk notifications. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("bulk")]
    public async Task<IActionResult> CreateBulk([FromBody] NotificationBulkCreateDto dto)
    {
        var result = await _notificationService.CreateBulkAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete a notification.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _notificationService.DeleteAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
