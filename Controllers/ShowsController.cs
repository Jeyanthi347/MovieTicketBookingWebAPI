
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Shows;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly IShowService service;

    public ShowsController(IShowService service)
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

    // POST: api/shows
    // Create a new show and automatically generate ShowSeats
    [Authorize(Roles = "Admin,TheatreManager")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateShowDto dto)
    {
        try
        {
            var show = await service.CreateAsync(dto, GetManagerUserId());

            return CreatedAtAction(
                nameof(GetById),
                new { id = show.ShowId },
                show);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DbUpdateException)
        {
            return Conflict("Unable to create show. Check the Movie ID and Screen ID.");
        }
    }

    // GET: api/shows?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(
                "Page number must be at least 1 and page size must be between 1 and 100.");
        }

        var shows = await service.GetAllAsync(pageNumber, pageSize);

        return Ok(shows);
    }

    // GET: api/shows/1
    // Get show by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var show = await service.GetByIdAsync(id);

        if (show == null)
            return NotFound("Show not found.");

        return Ok(show);
    }

    // GET: api/shows/1/seats
    // Get all seats and their availability for a particular show
    [HttpGet("{showId}/seats")]
    public async Task<IActionResult> GetSeatLayout(int showId)
    {
        var seats = await service.GetSeatLayoutAsync(showId);

        if (seats == null)
            return NotFound("Show not found.");

        return Ok(seats);
    }

    // PUT: api/shows/1
    // Update show
    [Authorize(Roles = "Admin,TheatreManager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateShowDto dto)
    {
        try
        {
            var show = await service.UpdateAsync(id, dto, GetManagerUserId());

            if (show == null)
                return NotFound("Show not found.");

            return Ok(show);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DbUpdateException)
        {
            return Conflict("Unable to update show. Check the Movie ID and Screen ID.");
        }
    }

    // DELETE: api/shows/1
    // Delete show
    [Authorize(Roles = "Admin,TheatreManager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await service.DeleteAsync(id, GetManagerUserId());

            if (!deleted)
                return NotFound("Show not found.");

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (DbUpdateException)
        {
            return Conflict("This show cannot be deleted because it is being used by another record.");
        }
    }
}
