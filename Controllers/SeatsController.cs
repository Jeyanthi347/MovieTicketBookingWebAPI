using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.DTOs;
using MovieTicketBookingWebAPI.DTOs.Seats;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly ISeatService service;

    public SeatsController(ISeatService service)
    {
        this.service = service;
    }

    // POST: api/seats
    // Create one seat
    [HttpPost]
    public async Task<IActionResult> Create(CreateSeatDto dto)
    {
        try
        {
            var seat = await service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = seat.SeatId },
                seat);
        }
        catch (DbUpdateException)
        {
            return Conflict("Unable to create seat. Check the screen ID and seat details.");
        }
    }

    // POST: api/seats/bulk
    // Create multiple seats
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate(BulkCreateSeatsDto dto)
    {
        try
        {
            var seats = await service.BulkCreateAsync(dto);

            return StatusCode(201, seats);
        }
        catch (DbUpdateException)
        {
            return Conflict("Unable to create seats. Check the screen ID and duplicate seat details.");
        }
    }

    // GET: api/seats/screen/1
    // Get all seats belonging to a screen
    [HttpGet("screen/{screenId}")]
    public async Task<IActionResult> GetByScreenId(int screenId)
    {
        var seats = await service.GetByScreenIdAsync(screenId);

        return Ok(seats);
    }

    // GET: api/seats/1
    // Get seat by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var seat = await service.GetByIdAsync(id);

        if (seat == null)
            return NotFound("Seat not found.");

        return Ok(seat);
    }

    // PUT: api/seats/1
    // Update seat
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSeatDto dto)
    {
        try
        {
            var seat = await service.UpdateAsync(id, dto);

            if (seat == null)
                return NotFound("Seat not found.");

            return Ok(seat);
        }
        catch (DbUpdateException)
        {
            return Conflict("Unable to update seat. Check the screen ID and duplicate seat details.");
        }
    }

    // DELETE: api/seats/1
    // Delete seat
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await service.DeleteAsync(id);

            if (!deleted)
                return NotFound("Seat not found.");

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict("This seat cannot be deleted because it is being used by another record.");
        }
    }
}