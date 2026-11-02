
using MovieTicketBookingWebAPI.DTOs.Ratings;

namespace MovieTicketBookingWebAPI.Services.Interfaces
{
    public interface IRatingService
    {
        Task<List<RatingResponseDto>> GetAllAsync();

        Task<RatingResponseDto?> GetByIdAsync(int id);

        Task<List<RatingResponseDto>> GetMovieRatingsAsync(int movieId);

        Task<List<RatingResponseDto>> GetTheatreRatingsAsync(int theatreId);

        Task<RatingResponseDto?> CreateAsync(CreateRatingDto dto);

        Task<RatingResponseDto?> UpdateAsync(int id, UpdateRatingDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
