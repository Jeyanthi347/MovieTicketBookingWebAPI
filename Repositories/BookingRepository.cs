
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;

namespace MovieTicketBookingWebAPI.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext context;

    public BookingRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    // Get show by ID
    public async Task<Show?> GetShowByIdAsync(int showId)
    {
        return await context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == showId);
    }

    // Get selected seats belonging to a particular show
    public async Task<List<ShowSeat>> GetShowSeatsByIdsAsync(
        int showId,
        List<int> seatIds)
    {
        return await context.ShowSeats
            .Where(ss => ss.ShowId == showId &&
                         seatIds.Contains(ss.SeatId))
            .ToListAsync();
    }

    // Get paginated bookings, optionally filtered by user
    public async Task<(List<Booking> Bookings, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize,
        int? userId = null)
    {
        var query = context.Bookings
            .Where(b => userId == null || b.UserId == userId);

        int totalRecords = await query.CountAsync();

        var bookings = await query
            .OrderByDescending(b => b.BookingId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (bookings, totalRecords);
    }

    // Get booking by ID
    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await context.Bookings
            .FirstOrDefaultAsync(b => b.BookingId == id);
    }

    // Get paginated bookings for a particular show and optionally user
    public async Task<(List<Booking> Bookings, int TotalRecords)> GetByShowIdAsync(
        int showId,
        int pageNumber,
        int pageSize,
        int? userId = null)
    {
        var query = context.Bookings
            .Where(b => b.ShowId == showId &&
                        (userId == null || b.UserId == userId));

        int totalRecords = await query.CountAsync();

        var bookings = await query
            .OrderByDescending(b => b.BookingId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (bookings, totalRecords);
    }

    // Get seat labels for a booking
    public async Task<List<string>> GetSeatLabelsAsync(int bookingId)
    {
        return await (
            from bookingSeat in context.BookingSeats
            join seat in context.Seats
                on bookingSeat.SeatId equals seat.SeatId
            where bookingSeat.BookingId == bookingId
            orderby seat.RowName, seat.SeatNumber
            select seat.RowName + seat.SeatNumber
        ).ToListAsync();
    }

    // Get show seats associated with a booking
    public async Task<List<ShowSeat>> GetShowSeatsForBookingAsync(
        Booking booking)
    {
        return await context.ShowSeats
            .Where(ss => ss.ShowId == booking.ShowId &&
                context.BookingSeats.Any(bs =>
                    bs.BookingId == booking.BookingId &&
                    bs.SeatId == ss.SeatId))
            .ToListAsync();
    }

    // Create booking and reserve seats
    public async Task<Booking> CreateAsync(
        Booking booking,
        List<BookingSeat> bookingSeats,
        List<ShowSeat> showSeats)
    {
        await using var transaction =
            await context.Database.BeginTransactionAsync();

        try
        {
            // Step 1: Set a short temporary reference
            booking.BookingReference = "TEMP";

            // Step 2: Save booking to generate BookingId
            context.Bookings.Add(booking);

            await context.SaveChangesAsync();

            // Step 3: Generate final booking reference
            booking.BookingReference = "MOV" + booking.BookingId;

            // Step 4: Assign BookingId to every BookingSeat
            foreach (var bookingSeat in bookingSeats)
            {
                bookingSeat.BookingId = booking.BookingId;
            }

            // Step 5: Add BookingSeat records
            context.BookingSeats.AddRange(bookingSeats);

            // Step 6: Mark selected ShowSeats as Booked
            foreach (var showSeat in showSeats)
            {
                showSeat.Status = "Booked";
            }

            // Step 7: Save all changes
            await context.SaveChangesAsync();

            // Step 8: Commit transaction
            await transaction.CommitAsync();

            return booking;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Cancel booking and release seats
    public async Task CancelAsync(
        Booking booking,
        List<ShowSeat> showSeats)
    {
        await using var transaction =
            await context.Database.BeginTransactionAsync();

        try
        {
            // Step 1: Change booking status
            booking.Status = "Cancelled";

            // Step 2: Make selected seats Available
            foreach (var showSeat in showSeats)
            {
                showSeat.Status = "Available";
            }

            // Step 3: Save changes
            await context.SaveChangesAsync();

            // Step 4: Commit transaction
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
