
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

    // Get paginated theatres
    public async Task<(List<Theatre> Theatres, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize)
    {
        int totalRecords = await context.Theatres.CountAsync();

        var theatres = await context.Theatres
            .OrderBy(t => t.TheatreId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (theatres, totalRecords);
    }

    // Get theatre by ID
    public async Task<Theatre?> GetByIdAsync(int id)
    {
        return await context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreId == id);
    }

    // Get theatre assigned to a particular manager
    public async Task<Theatre?> GetByManagerUserIdAsync(int userId)
    {
        return await context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreManagerUserId == userId);
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
