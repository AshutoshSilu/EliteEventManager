using System.Security.Claims;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Review;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Review and rating endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _reviewService.GetAllAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _reviewService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get reviews for a specific entity (Event, Venue, Vendor, Package).
    /// </summary>
    [HttpGet("entity/{entityType}/{entityId:int}")]
    public async Task<IActionResult> GetByEntity(string entityType, int entityId)
    {
        var result = await _reviewService.GetByEntityAsync(entityType, entityId);
        return Ok(result);
    }

    /// <summary>
    /// Submit a new review. Customer only.
    /// </summary>
    [Authorize(Policy = "CustomerOnly")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
    {
        var customerId = 1; // Placeholder
        var result = await _reviewService.CreateAsync(dto, customerId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Reply to a review. Manager/Admin only.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost("reply")]
    public async Task<IActionResult> Reply([FromBody] ReviewReplyDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _reviewService.ReplyAsync(dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Approve a review. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _reviewService.ApproveAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get pending reviews for moderation.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _reviewService.GetPendingAsync();
        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reviewService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
