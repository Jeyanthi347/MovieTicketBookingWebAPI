
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Screens;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScreensController : ControllerBase
{
    private readonly IScreenService service;

    public ScreensController(IScreenService service)
    {
        this.service = service;
    }

    // Get the logged-in TheatreManager's UserId
    private int? GetManagerUserId()
    {
        if (User.IsInRole("TheatreManager"))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userId, out int id) ? id : null;
        }

        return null;
    }

    // POST: api/screens
    [Authorize(Roles = "Admin,TheatreManager")]
    [HttpPost]
    public async Task<IActionResult> CreateScreen(CreateScreenDto dto)
    {
        try
        {
            var screen = await service.CreateAsync(dto, GetManagerUserId());

            return CreatedAtAction(
                nameof(GetScreenById),
                new { id = screen.ScreenId },
                screen);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    // GET: api/screens?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAllScreens(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(
                "Page number must be at least 1 and page size must be between 1 and 100.");
        }

        var screens = await service.GetAllAsync(pageNumber, pageSize);

        return Ok(screens);
    }

    // GET: api/screens/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScreenById(int id)
    {
        var screen = await service.GetByIdAsync(id);

        if (screen == null)
        {
            return NotFound("Screen not found.");
        }

        return Ok(screen);
    }

    // PUT: api/screens/1
    [Authorize(Roles = "Admin,TheatreManager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateScreen(int id, UpdateScreenDto dto)
    {
        try
        {
            var screen = await service.UpdateAsync(id, dto, GetManagerUserId());

            if (screen == null)
            {
                return NotFound("Screen not found.");
            }

            return Ok(screen);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    // DELETE: api/screens/1
    [Authorize(Roles = "Admin,TheatreManager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteScreen(int id)
    {
        try
        {
            var result = await service.DeleteAsync(id, GetManagerUserId());

            if (!result)
            {
                return NotFound("Screen not found.");
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (DbUpdateException)
        {
            return Conflict(
                "Cannot delete this screen because it is associated with other records.");
        }
    }
}
