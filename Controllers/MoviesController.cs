
using Microsoft.AspNetCore.Mvc;
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

    // GET: api/Movies
    [HttpGet]
    public async Task<ActionResult<List<MovieResponseDto>>> GetAll()
    {
        var movies = await movieService.GetAllAsync();

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
