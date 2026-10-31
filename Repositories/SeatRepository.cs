using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;

namespace MovieTicketBookingWebAPI.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly ApplicationDbContext context;

    public SeatRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    // Get all seats belonging to a screen
    public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
    {
        return await context.Seats
            .Where(s => s.ScreenId == screenId)
            .ToListAsync();
    }

    // Get seat by ID
    public async Task<Seat?> GetByIdAsync(int id)
    {
        return await context.Seats
            .FirstOrDefaultAsync(s => s.SeatId == id);
    }

    // Add one seat
    public async Task<Seat> AddAsync(Seat seat)
    {
        await context.Seats.AddAsync(seat);

        await context.SaveChangesAsync();

        return seat;
    }

    // Add multiple seats
    public async Task<List<Seat>> AddRangeAsync(List<Seat> seats)
    {
        await context.Seats.AddRangeAsync(seats);

        await context.SaveChangesAsync();

        return seats;
    }

    // Update an existing seat
    public async Task<Seat> UpdateAsync(Seat seat)
    {
        context.Seats.Update(seat);

        await context.SaveChangesAsync();

        return seat;
    }

    // Delete a seat
    public async Task<bool> DeleteAsync(int id)
    {
        var seat = await context.Seats.FindAsync(id);

        if (seat == null)
        {
            return false;
        }

        context.Seats.Remove(seat);

        await context.SaveChangesAsync();

        return true;
    }
}