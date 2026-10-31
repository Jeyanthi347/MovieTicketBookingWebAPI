using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.DTOs;
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

    // POST: api/shows
    // Create a new show and automatically generate ShowSeats
    [HttpPost]
    public async Task<IActionResult> Create(CreateShowDto dto)
    {
        try
        {
            var show = await service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = show.ShowId },
                show);
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

    // GET: api/shows
    // Get all shows
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var shows = await service.GetAllAsync();

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
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateShowDto dto)
    {
        try
        {
            var show = await service.UpdateAsync(id, dto);

            if (show == null)
                return NotFound("Show not found.");

            return Ok(show);
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await service.DeleteAsync(id);

            if (!deleted)
                return NotFound("Show not found.");

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict("This show cannot be deleted because it is being used by another record.");
        }
    }
}