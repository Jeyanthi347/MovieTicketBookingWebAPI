
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Theatres;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        this.theatreService = theatreService;
    }

    // GET: api/Theatres?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PaginationResponseDto<TheatreResponseDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(
                "Page number must be at least 1 and page size must be between 1 and 100.");
        }

        var theatres = await theatreService.GetAllAsync(pageNumber, pageSize);

        return Ok(theatres);
    }

    // GET: api/Theatres/1
    [HttpGet("{id}")]
    public async Task<ActionResult<TheatreResponseDto>> GetById(int id)
    {
        var theatre = await theatreService.GetByIdAsync(id);

        if (theatre == null)
        {
            return NotFound("Theatre not found.");
        }

        return Ok(theatre);
    }

    // POST: api/Theatres
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<TheatreResponseDto>> Create(
        CreateTheatreDto dto)
    {
        var theatre = await theatreService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = theatre.TheatreId },
            theatre);
    }

    // PUT: api/Theatres/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<TheatreResponseDto>> Update(
        int id,
        UpdateTheatreDto dto)
    {
        var theatre = await theatreService.UpdateAsync(id, dto);

        if (theatre == null)
        {
            return NotFound("Theatre not found.");
        }

        return Ok(theatre);
    }

    // DELETE: api/Theatres/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await theatreService.DeleteAsync(id);

        if (!result)
        {
            return NotFound("Theatre not found.");
        }

        return NoContent();
    }
}
