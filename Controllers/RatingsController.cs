
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingWebAPI.DTOs.Ratings;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService service;

        public RatingsController(IRatingService service)
        {
            this.service = service;
        }

        // GET: api/ratings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ratings = await service.GetAllAsync();

            return Ok(ratings);
        }

        // GET: api/ratings/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await service.GetByIdAsync(id);

            if (rating == null)
            {
                return NotFound("Rating not found.");
            }

            return Ok(rating);
        }

        // GET: api/ratings/movies/1
        [HttpGet("movies/{movieId}")]
        public async Task<IActionResult> GetMovieRatings(int movieId)
        {
            var ratings = await service.GetMovieRatingsAsync(movieId);

            return Ok(ratings);
        }

        // GET: api/ratings/theatres/1
        [HttpGet("theatres/{theatreId}")]
        public async Task<IActionResult> GetTheatreRatings(int theatreId)
        {
            var ratings = await service.GetTheatreRatingsAsync(theatreId);

            return Ok(ratings);
        }

        // POST: api/ratings
        [HttpPost]
        public async Task<IActionResult> Create(CreateRatingDto dto)
        {
            var rating = await service.CreateAsync(dto);

            if (rating == null)
            {
                return BadRequest(
                    "Invalid rating, booking, movie or theatre. " +
                    "Check the booking status and submitted details.");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = rating.RatingId },
                rating);
        }

        // PUT: api/ratings/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateRatingDto dto)
        {
            var rating = await service.UpdateAsync(id, dto);

            if (rating == null)
            {
                return BadRequest(
                    "Rating not found or invalid rating value.");
            }

            return Ok(rating);
        }

        // DELETE: api/ratings/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Rating not found.");
            }

            return Ok("Rating deleted successfully.");
        }
    }
}
