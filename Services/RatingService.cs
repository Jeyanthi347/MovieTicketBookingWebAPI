
using MovieTicketBookingWebAPI.DTOs.Common;
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

        // Get all ratings with pagination
        public async Task<PaginationResponseDto<RatingResponseDto>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            var result = await repository.GetAllAsync(
                pageNumber, pageSize);

            return CreatePaginationResponse(
                result.Ratings,
                result.TotalRecords,
                pageNumber,
                pageSize);
        }

        // Get rating by ID
        public async Task<RatingResponseDto?> GetByIdAsync(int id)
        {
            var rating = await repository.GetByIdAsync(id);

            return rating == null ? null : MapToDto(rating);
        }

        // Get movie ratings with pagination
        public async Task<PaginationResponseDto<RatingResponseDto>> GetMovieRatingsAsync(
            int movieId,
            int pageNumber,
            int pageSize)
        {
            var result = await repository.GetMovieRatingsAsync(
                movieId, pageNumber, pageSize);

            return CreatePaginationResponse(
                result.Ratings,
                result.TotalRecords,
                pageNumber,
                pageSize);
        }

        // Get theatre ratings with pagination
        public async Task<PaginationResponseDto<RatingResponseDto>> GetTheatreRatingsAsync(
            int theatreId,
            int pageNumber,
            int pageSize)
        {
            var result = await repository.GetTheatreRatingsAsync(
                theatreId, pageNumber, pageSize);

            return CreatePaginationResponse(
                result.Ratings,
                result.TotalRecords,
                pageNumber,
                pageSize);
        }

        // Create pagination response
        private static PaginationResponseDto<RatingResponseDto>
            CreatePaginationResponse(
                List<Rating> ratings,
                int totalRecords,
                int pageNumber,
                int pageSize)
        {
            return new PaginationResponseDto<RatingResponseDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    totalRecords / (double)pageSize),
                Data = ratings.Select(MapToDto).ToList()
            };
        }

        // Create rating
        public async Task<RatingResponseDto?> CreateAsync(
            CreateRatingDto dto, int? userId, bool isAdmin)
        {
            if (dto.MovieId.HasValue == dto.TheatreId.HasValue)
                throw new ArgumentException(
                    "Provide either MovieId or TheatreId, not both.");

            if (dto.RatingValue < 1 || dto.RatingValue > 10)
                throw new ArgumentException(
                    "Rating must be between 1 and 10.");

            if (!await repository.IsBookingConfirmedAsync(dto.BookingId))
                throw new ArgumentException(
                    "Only confirmed bookings can be rated.");

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

            if (!await repository.IsValidBookingTargetAsync(
                dto.BookingId, dto.MovieId, dto.TheatreId))
            {
                throw new ArgumentException(
                    "The selected movie or theatre does not belong to this booking.");
            }

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

        // Update rating
        public async Task<RatingResponseDto?> UpdateAsync(
            int id, UpdateRatingDto dto, int? userId)
        {
            if (dto.RatingValue < 1 || dto.RatingValue > 10)
                throw new ArgumentException(
                    "Rating must be between 1 and 10.");

            var rating = await repository.GetByIdAsync(id);

            if (rating == null)
                return null;

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

        // Delete rating
        public async Task<bool> DeleteAsync(int id, int? userId)
        {
            var rating = await repository.GetByIdAsync(id);

            if (rating == null)
                return false;

            if (userId.HasValue &&
                rating.Booking.UserId != userId.Value)
            {
                throw new UnauthorizedAccessException(
                    "You can delete only your own ratings.");
            }

            return await repository.DeleteAsync(id);
        }

        // Map entity to response DTO
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
