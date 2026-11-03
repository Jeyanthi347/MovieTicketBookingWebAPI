
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;

namespace MovieTicketBookingWebAPI.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext context;

        public RatingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        // Get all ratings with pagination
        public async Task<(List<Rating> Ratings, int TotalRecords)> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            var query = context.Ratings
                .Include(r => r.Booking)
                .Include(r => r.Movie)
                .Include(r => r.Theatre);

            int totalRecords = await query.CountAsync();

            var ratings = await query
                .OrderByDescending(r => r.RatingId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ratings, totalRecords);
        }

        // Get rating by ID
        public async Task<Rating?> GetByIdAsync(int id)
        {
            return await context.Ratings
                .Include(r => r.Booking)
                .Include(r => r.Movie)
                .Include(r => r.Theatre)
                .FirstOrDefaultAsync(r => r.RatingId == id);
        }

        // Get movie ratings with pagination
        public async Task<(List<Rating> Ratings, int TotalRecords)> GetMovieRatingsAsync(
            int movieId,
            int pageNumber,
            int pageSize)
        {
            var query = context.Ratings
                .Include(r => r.Booking)
                .Include(r => r.Movie)
                .Include(r => r.Theatre)
                .Where(r => r.MovieId == movieId);

            int totalRecords = await query.CountAsync();

            var ratings = await query
                .OrderByDescending(r => r.RatingId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ratings, totalRecords);
        }

        // Get theatre ratings with pagination
        public async Task<(List<Rating> Ratings, int TotalRecords)> GetTheatreRatingsAsync(
            int theatreId,
            int pageNumber,
            int pageSize)
        {
            var query = context.Ratings
                .Include(r => r.Booking)
                .Include(r => r.Movie)
                .Include(r => r.Theatre)
                .Where(r => r.TheatreId == theatreId);

            int totalRecords = await query.CountAsync();

            var ratings = await query
                .OrderByDescending(r => r.RatingId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ratings, totalRecords);
        }

        // Create rating
        public async Task<Rating> CreateAsync(Rating rating)
        {
            await context.Ratings.AddAsync(rating);
            await context.SaveChangesAsync();

            return rating;
        }

        // Update rating
        public async Task<Rating?> UpdateAsync(Rating rating)
        {
            context.Ratings.Update(rating);
            await context.SaveChangesAsync();

            return await GetByIdAsync(rating.RatingId);
        }

        // Delete rating
        public async Task<bool> DeleteAsync(int id)
        {
            var rating = await context.Ratings.FindAsync(id);

            if (rating == null)
                return false;

            context.Ratings.Remove(rating);
            await context.SaveChangesAsync();

            return true;
        }

        // Check whether booking is confirmed
        public async Task<bool> IsBookingConfirmedAsync(int bookingId)
        {
            return await context.Bookings.AnyAsync(b =>
                b.BookingId == bookingId &&
                b.Status == "Confirmed");
        }

        // Validate whether selected movie or theatre belongs to booking
        public async Task<bool> IsValidBookingTargetAsync(
            int bookingId,
            int? movieId,
            int? theatreId)
        {
            if (movieId.HasValue)
            {
                return await (
                    from booking in context.Bookings
                    join show in context.Shows
                        on booking.ShowId equals show.ShowId
                    where booking.BookingId == bookingId
                          && show.MovieId == movieId.Value
                    select booking
                ).AnyAsync();
            }

            if (theatreId.HasValue)
            {
                return await (
                    from booking in context.Bookings
                    join show in context.Shows
                        on booking.ShowId equals show.ShowId
                    join screen in context.Screens
                        on show.ScreenId equals screen.ScreenId
                    where booking.BookingId == bookingId
                          && screen.TheatreId == theatreId.Value
                    select booking
                ).AnyAsync();
            }

            return false;
        }

        // Check whether rating already exists
        public async Task<bool> RatingExistsAsync(
            int bookingId,
            int? movieId,
            int? theatreId)
        {
            return await context.Ratings.AnyAsync(r =>
                r.BookingId == bookingId &&
                r.MovieId == movieId &&
                r.TheatreId == theatreId);
        }

        // Check whether booking belongs to customer
        public async Task<bool> IsBookingOwnedByUserAsync(
            int bookingId,
            int userId)
        {
            return await context.Bookings.AnyAsync(b =>
                b.BookingId == bookingId &&
                b.UserId == userId);
        }
    }
}
