using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;

namespace MovieTicketBookingWebAPI.Repositories;

public class TheatreRepository : ITheatreRepository
{
    private readonly ApplicationDbContext context;

    public TheatreRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    // Get all theatres
    public async Task<List<Theatre>> GetAllAsync()
    {
        return await context.Theatres.ToListAsync();
    }

    // Get theatre by ID
    public async Task<Theatre?> GetByIdAsync(int id)
    {
        return await context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreId == id);
    }

    // Add a new theatre
    public async Task<Theatre> AddAsync(Theatre theatre)
    {
        await context.Theatres.AddAsync(theatre);

        await context.SaveChangesAsync();

        return theatre;
    }

    // Update theatre
    public async Task<Theatre> UpdateAsync(Theatre theatre)
    {
        context.Theatres.Update(theatre);

        await context.SaveChangesAsync();

        return theatre;
    }

    // Delete theatre
    public async Task<bool> DeleteAsync(int id)
    {
        var theatre = await context.Theatres.FindAsync(id);

        if (theatre == null)
        {
            return false;
        }

        context.Theatres.Remove(theatre);

        await context.SaveChangesAsync();

        return true;
    }
}