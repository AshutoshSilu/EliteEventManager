using EliteEvents.Application.Common;
using EliteEvents.Application.DTOs.Employee;
using EliteEvents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteEvents.API.Controllers;

/// <summary>
/// Employee registration and lifecycle management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters parameters)
    {
        var result = await _employeeService.GetAllAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var result = await _employeeService.GetByUserIdAsync(userId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] EmployeeRegistrationDto dto)
    {
        var result = await _employeeService.RegisterAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> Update(Guid userId, [FromBody] EmployeeUpdateDto dto)
    {
        var result = await _employeeService.UpdateAsync(userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{userId:guid}/onboard")]
    public async Task<IActionResult> Onboard(Guid userId)
    {
        var result = await _employeeService.OnboardAsync(userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{userId:guid}/resign")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Resign(Guid userId)
    {
        var result = await _employeeService.ResignAsync(userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{userId:guid}/terminate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Terminate(Guid userId)
    {
        var result = await _employeeService.TerminateAsync(userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
