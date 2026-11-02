
using MovieTicketBookingWebAPI.DTOs.Ratings;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository repository;

        public RatingService(IRatingRepository repository)
        {
            this.repository = repository;
        }

        // Get all ratings
        public async Task<List<RatingResponseDto>> GetAllAsync()
        {
            var ratings = await repository.GetAllAsync();

            return ratings.Select(r => MapToDto(r)).ToList();
        }

        // Get rating by ID
        public async Task<RatingResponseDto?> GetByIdAsync(int id)
        {
            var rating = await repository.GetByIdAsync(id);

            if (rating == null)
            {
                return null;
            }

            return MapToDto(rating);
        }

        // Get movie ratings
        public async Task<List<RatingResponseDto>> GetMovieRatingsAsync(int movieId)
        {
            var ratings = await repository.GetMovieRatingsAsync(movieId);

            return ratings.Select(r => MapToDto(r)).ToList();
        }

        // Get theatre ratings
        public async Task<List<RatingResponseDto>> GetTheatreRatingsAsync(int theatreId)
        {
            var ratings = await repository.GetTheatreRatingsAsync(theatreId);

            return ratings.Select(r => MapToDto(r)).ToList();
        }

        // Create rating
        public async Task<RatingResponseDto?> CreateAsync(CreateRatingDto dto)
        {
            // Validate that exactly one target is selected
            if (dto.MovieId.HasValue == dto.TheatreId.HasValue)
            {
                return null;
            }

            // Validate rating value
            if (dto.RatingValue < 1 || dto.RatingValue > 10)
            {
                return null;
            }

            // Validate booking status
            bool isConfirmed =
                await repository.IsBookingConfirmedAsync(dto.BookingId);

            if (!isConfirmed)
            {
                return null;
            }

            // Validate selected movie or theatre belongs to booking
            bool isValidTarget =
                await repository.IsValidBookingTargetAsync(
                    dto.BookingId,
                    dto.MovieId,
                    dto.TheatreId);

            if (!isValidTarget)
            {
                return null;
            }

            // Prevent duplicate ratings
            bool ratingExists =
                await repository.RatingExistsAsync(
                    dto.BookingId,
                    dto.MovieId,
                    dto.TheatreId);

            if (ratingExists)
            {
                return null;
            }

            var rating = new Rating
            {
                BookingId = dto.BookingId,
                MovieId = dto.MovieId,
                TheatreId = dto.TheatreId,
                RatingValue = dto.RatingValue,
                Review = dto.Review
            };

            var createdRating = await repository.CreateAsync(rating);

            var result = await repository.GetByIdAsync(
                createdRating.RatingId);

            return result == null ? null : MapToDto(result);
        }

        // Update rating
        public async Task<RatingResponseDto?> UpdateAsync(
            int id,
            UpdateRatingDto dto)
        {
            if (dto.RatingValue < 1 || dto.RatingValue > 10)
            {
                return null;
            }

            var rating = await repository.GetByIdAsync(id);

            if (rating == null)
            {
                return null;
            }

            rating.RatingValue = dto.RatingValue;
            rating.Review = dto.Review;

            var updatedRating = await repository.UpdateAsync(id, rating);

            if (updatedRating == null)
            {
                return null;
            }

            return MapToDto(updatedRating);
        }

        // Delete rating
        public async Task<bool> DeleteAsync(int id)
        {
            return await repository.DeleteAsync(id);
        }

        // Convert Rating model into RatingResponseDto
        private static RatingResponseDto MapToDto(Rating rating)
        {
            return new RatingResponseDto
            {
                RatingId = rating.RatingId,
                BookingId = rating.BookingId,
                CustomerName = rating.Booking.CustomerName,
                MovieName = rating.Movie?.MovieName,
                TheatreName = rating.Theatre?.Name,
                RatingValue = rating.RatingValue,
                Review = rating.Review
            };
        }
    }
}
