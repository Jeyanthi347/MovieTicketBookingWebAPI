using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // POST: api/screens
    [HttpPost]
    public async Task<IActionResult> CreateScreen(CreateScreenDto dto)
    {
        var screen = await service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetScreenById),
            new { id = screen.ScreenId },
            screen);
    }

    // GET: api/screens
    [HttpGet]
    public async Task<IActionResult> GetAllScreens()
    {
        var screens = await service.GetAllAsync();

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
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateScreen(int id, UpdateScreenDto dto)
    {
        var screen = await service.UpdateAsync(id, dto);

        if (screen == null)
        {
            return NotFound("Screen not found.");
        }

        return Ok(screen);
    }

    // DELETE: api/screens/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteScreen(int id)
    {
        try
        {
            var result = await service.DeleteAsync(id);

            if (!result)
            {
                return NotFound("Screen not found.");
            }

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(
                "Cannot delete this screen because it is associated with other records.");
        }
    }
}