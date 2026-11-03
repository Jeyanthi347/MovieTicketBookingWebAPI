
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<(List<Rating> Ratings, int TotalRecords)> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<Rating?> GetByIdAsync(int id);

        Task<(List<Rating> Ratings, int TotalRecords)> GetMovieRatingsAsync(
            int movieId,
            int pageNumber,
            int pageSize);

        Task<(List<Rating> Ratings, int TotalRecords)> GetTheatreRatingsAsync(
            int theatreId,
            int pageNumber,
            int pageSize);

        Task<Rating> CreateAsync(Rating rating);
        Task<Rating?> UpdateAsync(Rating rating);
        Task<bool> DeleteAsync(int id);

        Task<bool> IsBookingConfirmedAsync(int bookingId);

        Task<bool> IsValidBookingTargetAsync(
            int bookingId, int? movieId, int? theatreId);

        Task<bool> RatingExistsAsync(
            int bookingId, int? movieId, int? theatreId);

        Task<bool> IsBookingOwnedByUserAsync(
            int bookingId, int userId);
    }
}
