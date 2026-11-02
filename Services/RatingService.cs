
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

        public async Task<List<RatingResponseDto>> GetAllAsync()
        {
            var ratings = await repository.GetAllAsync();

            return ratings.Select(MapToDto).ToList();
        }

        public async Task<RatingResponseDto?> GetByIdAsync(int id)
        {
            var rating = await repository.GetByIdAsync(id);

            return rating == null ? null : MapToDto(rating);
        }

        public async Task<List<RatingResponseDto>> GetMovieRatingsAsync(
            int movieId)
        {
            var ratings = await repository.GetMovieRatingsAsync(movieId);

            return ratings.Select(MapToDto).ToList();
        }

        public async Task<List<RatingResponseDto>> GetTheatreRatingsAsync(
            int theatreId)
        {
            var ratings = await repository.GetTheatreRatingsAsync(theatreId);

            return ratings.Select(MapToDto).ToList();
        }

        public async Task<RatingResponseDto?> CreateAsync(
            CreateRatingDto dto, int? userId, bool isAdmin)
        {
            // Exactly one target must be selected.
            if (dto.MovieId.HasValue == dto.TheatreId.HasValue)
                throw new ArgumentException(
                    "Provide either MovieId or TheatreId, not both.");

            // Rating must be between 1 and 10.
            if (dto.RatingValue < 1 || dto.RatingValue > 10)
                throw new ArgumentException(
                    "Rating must be between 1 and 10.");

            // Booking must be confirmed.
            if (!await repository.IsBookingConfirmedAsync(dto.BookingId))
                throw new ArgumentException(
                    "Only confirmed bookings can be rated.");

            // Customers can rate only their own bookings.
            if (!isAdmin)
            {
                if (!userId.HasValue ||
                    !await repository.IsBookingOwnedByUserAsync(
                        dto.BookingId, userId.Value))
                {
                    throw new UnauthorizedAccessException(
                        "You can rate only your own bookings.");
                }
            }

            // Verify that the booking belongs to the selected movie/theatre.
            if (!await repository.IsValidBookingTargetAsync(
                dto.BookingId, dto.MovieId, dto.TheatreId))
            {
                throw new ArgumentException(
                    "The selected movie or theatre does not belong to this booking.");
            }

            // Prevent duplicate ratings for the same booking and target.
            if (await repository.RatingExistsAsync(
                dto.BookingId, dto.MovieId, dto.TheatreId))
            {
                throw new ArgumentException(
                    "A rating already exists for this booking and target.");
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

        public async Task<RatingResponseDto?> UpdateAsync(
            int id, UpdateRatingDto dto, int? userId)
        {
            if (dto.RatingValue < 1 || dto.RatingValue > 10)
                throw new ArgumentException(
                    "Rating must be between 1 and 10.");

            var rating = await repository.GetByIdAsync(id);

            if (rating == null)
                return null;

            // Admin passes null and can update any rating.
            // Customers can update only their own ratings.
            if (userId.HasValue &&
                rating.Booking.UserId != userId.Value)
            {
                throw new UnauthorizedAccessException(
                    "You can update only your own ratings.");
            }

            rating.RatingValue = dto.RatingValue;
            rating.Review = dto.Review;

            var updatedRating = await repository.UpdateAsync(rating);

            return updatedRating == null
                ? null
                : MapToDto(updatedRating);
        }

        public async Task<bool> DeleteAsync(int id, int? userId)
        {
            var rating = await repository.GetByIdAsync(id);

            if (rating == null)
                return false;

            // Admin passes null and can delete any rating.
            // Customers can delete only their own ratings.
            if (userId.HasValue &&
                rating.Booking.UserId != userId.Value)
            {
                throw new UnauthorizedAccessException(
                    "You can delete only your own ratings.");
            }

            return await repository.DeleteAsync(id);
        }

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
