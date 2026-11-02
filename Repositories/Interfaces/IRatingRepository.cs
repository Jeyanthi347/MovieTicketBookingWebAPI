using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<List<Rating>> GetAllAsync();
        Task<Rating?> GetByIdAsync(int id);
        Task<List<Rating>> GetMovieRatingsAsync(int movieId);
        Task<List<Rating>> GetTheatreRatingsAsync(int theatreId);

        Task<Rating> CreateAsync(Rating rating);
        Task<Rating?> UpdateAsync(Rating rating);
        Task<bool> DeleteAsync(int id);

        Task<bool> IsBookingConfirmedAsync(int bookingId);
        Task<bool> IsValidBookingTargetAsync(
            int bookingId, int? movieId, int? theatreId);

        Task<bool> RatingExistsAsync(
            int bookingId, int? movieId, int? theatreId);

        Task<bool> IsBookingOwnedByUserAsync(int bookingId, int userId);
    }
}