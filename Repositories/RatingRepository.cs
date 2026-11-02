
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

        // Get all ratings
        public async Task<List<Rating>> GetAllAsync()
        {
            return await context.Ratings
                .Include(r => r.Booking)
                .Include(r => r.Movie)
                .Include(r => r.Theatre)
                .ToListAsync();
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

        // Get movie ratings
        public async Task<List<Rating>> GetMovieRatingsAsync(int movieId)
        {
            return await context.Ratings
                .Include(r => r.Booking)
                .Include(r => r.Movie)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }

        // Get theatre ratings
        public async Task<List<Rating>> GetTheatreRatingsAsync(int theatreId)
        {
            return await context.Ratings
                .Include(r => r.Booking)
                .Include(r => r.Theatre)
                .Where(r => r.TheatreId == theatreId)
                .ToListAsync();
        }

        // Add rating
        public async Task<Rating> CreateAsync(Rating rating)
        {
            context.Ratings.Add(rating);
            await context.SaveChangesAsync();

            return rating;
        }

        // Update rating
        public async Task<Rating?> UpdateAsync(int id, Rating rating)
        {
            var existingRating = await context.Ratings
                .FirstOrDefaultAsync(r => r.RatingId == id);

            if (existingRating == null)
            {
                return null;
            }

            existingRating.RatingValue = rating.RatingValue;
            existingRating.Review = rating.Review;

            await context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        // Delete rating
        public async Task<bool> DeleteAsync(int id)
        {
            var rating = await context.Ratings
                .FirstOrDefaultAsync(r => r.RatingId == id);

            if (rating == null)
            {
                return false;
            }

            context.Ratings.Remove(rating);
            await context.SaveChangesAsync();

            return true;
        }

        // Check whether booking is confirmed
        public async Task<bool> IsBookingConfirmedAsync(int bookingId)
        {
            return await context.Bookings
                .AnyAsync(b =>
                    b.BookingId == bookingId &&
                    b.Status == "Confirmed");
        }

        // Check whether movie or theatre belongs to the booking
        public async Task<bool> IsValidBookingTargetAsync(
            int bookingId,
            int? movieId,
            int? theatreId)
        {
            // Validate movie
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

            // Validate theatre
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

        // Check duplicate rating
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
    }
}
