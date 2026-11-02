using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.DTOs.Theatres;
using MovieTicketBookingWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService service;

    public TheatresController(ITheatreService service)
    {
        this.service = service;
    }

    // POST: api/theatres
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateTheatre(CreateTheatreDto dto)
    {
        var theatre = await service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetTheatreById),
            new { id = theatre.TheatreId },
            theatre
        );
    }

    // GET: api/theatres
    [HttpGet]
    public async Task<IActionResult> GetAllTheatres()
    {
        var theatres = await service.GetAllAsync();

        return Ok(theatres);
    }

    // GET: api/theatres/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTheatreById(int id)
    {
        var theatre = await service.GetByIdAsync(id);

        if (theatre == null)
        {
            return NotFound("Theatre not found.");
        }

        return Ok(theatre);
    }

    // PUT: api/theatres/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTheatre(
        int id,
        UpdateTheatreDto dto)
    {
        var theatre = await service.UpdateAsync(id, dto);

        if (theatre == null)
        {
            return NotFound("Theatre not found.");
        }

        return Ok(theatre);
    }

    // DELETE: api/theatres/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTheatre(int id)
    {
        try
        {
            var result = await service.DeleteAsync(id);

            if (!result)
            {
                return NotFound("Theatre not found.");
            }

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(
                "Cannot delete this theatre because it is associated with other records."
            );
        }
    }
}