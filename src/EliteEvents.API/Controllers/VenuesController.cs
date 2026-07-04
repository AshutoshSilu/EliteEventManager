using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Venue;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Venue management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    private readonly IVenueService _venueService;

    public VenuesController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    /// <summary>
    /// Get all venues with pagination and filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _venueService.GetAllAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Get venue by ID with full details and images.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _venueService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new venue. Admin/Manager only.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VenueCreateDto dto)
    {
        var result = await _venueService.CreateAsync(dto);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result)
            : BadRequest(result);
    }

    /// <summary>
    /// Update an existing venue.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] VenueUpdateDto dto)
    {
        dto.Id = id;
        var result = await _venueService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Soft-delete a venue.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _venueService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get featured venues.
    /// </summary>
    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured([FromQuery] int count = 6)
    {
        var result = await _venueService.GetFeaturedAsync(count);
        return Ok(result);
    }

    /// <summary>
    /// Search venues with filters.
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int? minCapacity, [FromQuery] int? maxCapacity, [FromQuery] QueryParameters parameters)
    {
        var result = await _venueService.SearchAsync(q, minCapacity, maxCapacity, parameters);
        return Ok(result);
    }

    /// <summary>
    /// Check venue availability for a specific date.
    /// </summary>
    [HttpGet("{id:int}/availability")]
    public async Task<IActionResult> CheckAvailability(int id, [FromQuery] DateOnly date, [FromQuery] string? startTime, [FromQuery] string? endTime)
    {
        var result = await _venueService.CheckAvailabilityAsync(id, date, startTime, endTime);
        return Ok(result);
    }

    /// <summary>
    /// Get venue availability calendar for a date range.
    /// </summary>
    [HttpGet("{id:int}/calendar")]
    public async Task<IActionResult> GetCalendar(int id, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var result = await _venueService.GetAvailabilityAsync(id, startDate, endDate);
        return Ok(result);
    }
}
