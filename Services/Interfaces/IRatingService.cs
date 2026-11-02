using MovieTicketBookingWebAPI.DTOs.Ratings;

namespace MovieTicketBookingWebAPI.Services.Interfaces
{
    public interface IRatingService
    {
        Task<List<RatingResponseDto>> GetAllAsync();

        Task<RatingResponseDto?> GetByIdAsync(int id);

        Task<List<RatingResponseDto>> GetMovieRatingsAsync(int movieId);

        Task<List<RatingResponseDto>> GetTheatreRatingsAsync(int theatreId);

        Task<RatingResponseDto?> CreateAsync(
            CreateRatingDto dto, int? userId, bool isAdmin);

        Task<RatingResponseDto?> UpdateAsync(
            int id, UpdateRatingDto dto, int? userId);

        Task<bool> DeleteAsync(int id, int? userId);
    }
}