using System.Security.Claims;
using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Vendor;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Vendor management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VendorsController : ControllerBase
{
    private readonly IVendorService _vendorService;

    public VendorsController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _vendorService.GetAllAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _vendorService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VendorCreateDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _vendorService.CreateAsync(dto, userId);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result)
            : BadRequest(result);
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] VendorUpdateDto dto)
    {
        dto.Id = id;
        var result = await _vendorService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _vendorService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await _vendorService.GetByCategoryAsync(categoryId);
        return Ok(result);
    }

    [HttpGet("top-rated")]
    public async Task<IActionResult> GetTopRated([FromQuery] int count = 10)
    {
        var result = await _vendorService.GetTopRatedAsync(count);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _vendorService.GetCategoriesAsync();
        return Ok(result);
    }
}
