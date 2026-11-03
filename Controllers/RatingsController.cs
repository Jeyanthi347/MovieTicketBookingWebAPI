
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Ratings;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService ratingService;

        public RatingsController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        // Validate pagination values
        private IActionResult? ValidatePagination(
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(
                    "Page number must be at least 1 and page size must be between 1 and 100.");
            }

            return null;
        }

        // GET: api/Ratings?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var validation = ValidatePagination(pageNumber, pageSize);

            if (validation != null)
                return validation;

            return Ok(await ratingService.GetAllAsync(
                pageNumber, pageSize));
        }

        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await ratingService.GetByIdAsync(id);

            if (rating == null)
                return NotFound("Rating not found.");

            return Ok(rating);
        }

        // GET: api/Ratings/movie/1?pageNumber=1&pageSize=10
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetMovieRatings(
            int movieId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (movieId < 1)
                return BadRequest("Movie ID must be at least 1.");

            var validation = ValidatePagination(pageNumber, pageSize);

            if (validation != null)
                return validation;

            return Ok(await ratingService.GetMovieRatingsAsync(
                movieId, pageNumber, pageSize));
        }

        // GET: api/Ratings/theatre/1?pageNumber=1&pageSize=10
        [HttpGet("theatre/{theatreId}")]
        public async Task<IActionResult> GetTheatreRatings(
            int theatreId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (theatreId < 1)
                return BadRequest("Theatre ID must be at least 1.");

            var validation = ValidatePagination(pageNumber, pageSize);

            if (validation != null)
                return validation;

            return Ok(await ratingService.GetTheatreRatingsAsync(
                theatreId, pageNumber, pageSize));
        }

        // POST: api/Ratings
        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Create(CreateRatingDto dto)
        {
            try
            {
                int userId = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                bool isAdmin = User.IsInRole("Admin");

                var rating = await ratingService.CreateAsync(
                    dto, userId, isAdmin);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = rating!.RatingId },
                    rating);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Ratings/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Update(
            int id, UpdateRatingDto dto)
        {
            try
            {
                int? userId = User.IsInRole("Admin")
                    ? null
                    : int.Parse(
                        User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var rating = await ratingService.UpdateAsync(
                    id, dto, userId);

                if (rating == null)
                    return NotFound("Rating not found.");

                return Ok(rating);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int? userId = User.IsInRole("Admin")
                    ? null
                    : int.Parse(
                        User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                bool deleted = await ratingService.DeleteAsync(id, userId);

                if (!deleted)
                    return NotFound("Rating not found.");

                return Ok("Rating deleted successfully.");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
