using System.Security.Claims;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.User;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// User management and profile endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get all users. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _userService.GetAllAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// Get user by ID. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new user. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
    {
        var result = await _userService.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update user. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto dto)
    {
        var result = await _userService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Delete user. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get users by role. Admin only.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("role/{roleId:int}")]
    public async Task<IActionResult> GetByRole(int roleId)
    {
        var result = await _userService.GetByRoleAsync(roleId);
        return Ok(result);
    }

    /// <summary>
    /// Get current user's profile.
    /// </summary>
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _userService.GetProfileAsync(userId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Update current user's profile.
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _userService.UpdateProfileAsync(userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
