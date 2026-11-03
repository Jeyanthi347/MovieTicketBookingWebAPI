
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Bookings;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Customer")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService service;

    public BookingsController(IBookingService service)
    {
        this.service = service;
    }

    // Get the logged-in user's ID from the JWT token
    private int GetCurrentUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    // GET: api/bookings?pageNumber=1&pageSize=10
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

        int? userId = User.IsInRole("Admin")
            ? null
            : GetCurrentUserId();

        var bookings = await service.GetAllAsync(
            pageNumber,
            pageSize,
            userId);

        return Ok(bookings);
    }

    // GET: api/bookings/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        int? userId = User.IsInRole("Admin")
            ? null
            : GetCurrentUserId();

        var booking = await service.GetByIdAsync(id, userId);

        if (booking == null)
            return NotFound("Booking not found.");

        return Ok(booking);
    }

    // GET: api/bookings/show/5?pageNumber=1&pageSize=10
    [HttpGet("show/{showId}")]
    public async Task<IActionResult> GetByShowId(
        int showId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (showId < 1 || pageNumber < 1 ||
            pageSize < 1 || pageSize > 100)
        {
            return BadRequest(
                "Show ID and page number must be at least 1. Page size must be between 1 and 100.");
        }

        int? userId = User.IsInRole("Admin")
            ? null
            : GetCurrentUserId();

        var bookings = await service.GetByShowIdAsync(
            showId,
            pageNumber,
            pageSize,
            userId);

        return Ok(bookings);
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto)
    {
        try
        {
            int userId = GetCurrentUserId();

            var booking = await service.CreateAsync(dto, userId);

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
            int? userId = User.IsInRole("Admin")
                ? null
                : GetCurrentUserId();

            var result = await service.CancelAsync(id, userId);

            if (!result)
                return NotFound("Booking not found.");

            return Ok("Booking cancelled successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
