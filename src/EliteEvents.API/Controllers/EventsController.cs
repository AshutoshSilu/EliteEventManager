using System.Security.Claims;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Event;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Event management endpoints for CRUD operations and public listings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Get all events with pagination, search, sorting, and filtering.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _eventService.GetAllAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Get event by ID with full details.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _eventService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new event. Requires Manager or Admin role.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] EventCreateDto dto)
    {
        var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _eventService.CreateAsync(dto, organizerId);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result)
            : BadRequest(result);
    }

    /// <summary>
    /// Update an existing event.
    /// </summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] EventUpdateDto dto)
    {
        dto.Id = id;
        var result = await _eventService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Soft-delete an event.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _eventService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get featured events for the home page.
    /// </summary>
    [HttpGet("featured")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeatured([FromQuery] int count = 6)
    {
        var result = await _eventService.GetFeaturedAsync(count);
        return Ok(result);
    }

    /// <summary>
    /// Get upcoming events.
    /// </summary>
    [HttpGet("upcoming")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUpcoming([FromQuery] int count = 10)
    {
        var result = await _eventService.GetUpcomingAsync(count);
        return Ok(result);
    }

    /// <summary>
    /// Get events by category.
    /// </summary>
    [HttpGet("category/{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await _eventService.GetByCategoryAsync(categoryId);
        return Ok(result);
    }

    /// <summary>
    /// Search events by keyword.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] QueryParameters parameters)
    {
        var result = await _eventService.SearchAsync(q, parameters);
        return Ok(result);
    }

    /// <summary>
    /// Get all event categories.
    /// </summary>
    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _eventService.GetCategoriesAsync();
        return Ok(result);
    }
}
