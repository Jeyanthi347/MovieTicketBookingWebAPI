using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
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

    // Get all shows
    public async Task<List<Show>> GetAllAsync()
    {
        return await context.Shows.ToListAsync();
    }

    // Get show by ID
    public async Task<Show?> GetByIdAsync(int id)
    {
        return await context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == id);
    }

    // Add a new show
    public async Task<Show> AddAsync(Show show)
    {
        await context.Shows.AddAsync(show);
        await context.SaveChangesAsync();

        return show;
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