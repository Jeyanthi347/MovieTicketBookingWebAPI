
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

        Task<Rating?> UpdateAsync(int id, Rating rating);

        Task<bool> DeleteAsync(int id);

        // Booking validation
        Task<bool> IsBookingConfirmedAsync(int bookingId);

        // Validate movie or theatre against booking
        Task<bool> IsValidBookingTargetAsync(
            int bookingId,
            int? movieId,
            int? theatreId);

        // Check duplicate rating
        Task<bool> RatingExistsAsync(
            int bookingId,
            int? movieId,
            int? theatreId);
    }
}
