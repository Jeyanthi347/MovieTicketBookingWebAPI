
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Movies;
using MovieTicketBookingWebAPI.Services.Interfaces;



namespace MovieTicketBookingWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    
    private readonly IMovieService movieService;

    public MoviesController(IMovieService movieService)
    {
        this.movieService = movieService;
    }

    // GET: api/Movies?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PaginationResponseDto<MovieResponseDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(
                "Page number must be at least 1 and page size must be between 1 and 100.");
        }

        var movies = await movieService.GetAllAsync(pageNumber, pageSize);

        return Ok(movies);
    }

    // GET: api/Movies/1
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieResponseDto>> GetById(int id)
    {
        var movie = await movieService.GetByIdAsync(id);

        if (movie == null)
        {
            return NotFound("Movie not found.");
        }

        return Ok(movie);
    }

    // POST: api/Movies
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<MovieResponseDto>> Create(
        CreateMovieDto dto)
    {
        var movie = await movieService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = movie.MovieId },
            movie
        );
    }

    // PUT: api/Movies/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<MovieResponseDto>> Update(
        int id,
        UpdateMovieDto dto)
    {
        var movie = await movieService.UpdateAsync(id, dto);

        if (movie == null)
        {
            return NotFound("Movie not found.");
        }

        return Ok(movie);
    }

    // DELETE: api/Movies/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await movieService.DeleteAsync(id);

        if (!result)
        {
            return NotFound("Movie not found.");
        }

        return NoContent();
    }
}
