using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingWebAPI.DTOs.Bookings;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService service;

    public BookingsController(IBookingService service)
    {
        this.service = service;
    }

    // GET: api/bookings
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await service.GetAllAsync();
        return Ok(bookings);
    }

    // GET: api/bookings/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await service.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound("Booking not found.");
        }

        return Ok(booking);
    }

    // GET: api/bookings/show/5
    [HttpGet("show/{showId}")]
    public async Task<IActionResult> GetByShowId(int showId)
    {
        var bookings = await service.GetByShowIdAsync(showId);
        return Ok(bookings);
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto)
    {
        try
        {
            var booking = await service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = booking.BookingId },
                booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    // PUT: api/bookings/1/cancel
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var result = await service.CancelAsync(id);

            if (!result)
            {
                return NotFound("Booking not found.");
            }

            return Ok("Booking cancelled successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}