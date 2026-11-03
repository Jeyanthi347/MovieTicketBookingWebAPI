
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.DTOs.Shows;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;

namespace MovieTicketBookingWebAPI.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly ApplicationDbContext context;

    public ShowRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    // Get paginated shows
    public async Task<(List<Show> Shows, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize)
    {
        int totalRecords = await context.Shows.CountAsync();

        var shows = await context.Shows
            .OrderBy(s => s.ShowId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (shows, totalRecords);
    }

    // Get show by ID
    public async Task<Show?> GetByIdAsync(int id)
    {
        return await context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == id);
    }

    // Add a new show and automatically generate ShowSeats
    public async Task<Show> AddAsync(Show show)
    {
        // Get all physical seats belonging to this screen
        var seats = await context.Seats
            .Where(s => s.ScreenId == show.ScreenId)
            .ToListAsync();

        if (seats.Count == 0)
        {
            throw new InvalidOperationException(
                "No seats are available for this screen.");
        }

        // Start a database transaction
        await using var transaction =
            await context.Database.BeginTransactionAsync();

        try
        {
            // Save the show first to generate its ShowId
            await context.Shows.AddAsync(show);
            await context.SaveChangesAsync();

            // Create one ShowSeat for every physical seat
            var showSeats = new List<ShowSeat>();

            foreach (var seat in seats)
            {
                ShowSeat showSeat = new ShowSeat
                {
                    ShowId = show.ShowId,
                    SeatId = seat.SeatId,
                    Status = "Available"
                };

                showSeats.Add(showSeat);
            }

            // Save all generated ShowSeats
            await context.ShowSeats.AddRangeAsync(showSeats);
            await context.SaveChangesAsync();

            // Complete the transaction
            await transaction.CommitAsync();

            return show;
        }
        catch
        {
            // Undo database changes if anything fails
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Get the seat layout for a particular show
    public async Task<List<ShowSeatResponseDto>> GetSeatLayoutAsync(int showId)
    {
        var seatLayout = await (
            from showSeat in context.ShowSeats
            join seat in context.Seats
                on showSeat.SeatId equals seat.SeatId
            join show in context.Shows
                on showSeat.ShowId equals show.ShowId
            where showSeat.ShowId == showId
            orderby seat.RowName, seat.SeatNumber
            select new ShowSeatResponseDto
            {
                SeatId = seat.SeatId,
                RowName = seat.RowName,
                SeatNumber = seat.SeatNumber,
                SeatLabel = seat.RowName + seat.SeatNumber,
                SeatType = seat.SeatType,
                Price = show.TicketPrice,
                Status = showSeat.Status
            }
        ).ToListAsync();

        return seatLayout;
    }

    // Update an existing show
    public async Task<Show> UpdateAsync(Show show)
    {
        context.Shows.Update(show);

        await context.SaveChangesAsync();

        return show;
    }

    // Delete a show
    public async Task<bool> DeleteAsync(int id)
    {
        var show = await context.Shows.FindAsync(id);

        if (show == null)
        {
            return false;
        }

        context.Shows.Remove(show);

        await context.SaveChangesAsync();

        return true;
    }
}
