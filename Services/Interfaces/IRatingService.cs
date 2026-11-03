
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Ratings;

namespace MovieTicketBookingWebAPI.Services.Interfaces
{
    public interface IRatingService
    {
        Task<PaginationResponseDto<RatingResponseDto>> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<RatingResponseDto?> GetByIdAsync(int id);

        Task<PaginationResponseDto<RatingResponseDto>> GetMovieRatingsAsync(
            int movieId,
            int pageNumber,
            int pageSize);

        Task<PaginationResponseDto<RatingResponseDto>> GetTheatreRatingsAsync(
            int theatreId,
            int pageNumber,
            int pageSize);

        Task<RatingResponseDto?> CreateAsync(
            CreateRatingDto dto, int? userId, bool isAdmin);

        Task<RatingResponseDto?> UpdateAsync(
            int id, UpdateRatingDto dto, int? userId);

        Task<bool> DeleteAsync(int id, int? userId);
    }
}
