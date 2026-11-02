
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;

namespace MovieTicketBookingWebAPI.Repositories;

public class ScreenRepository : IScreenRepository
{
    private readonly ApplicationDbContext context;

    public ScreenRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    // Get all screens
    public async Task<List<Screen>> GetAllAsync()
    {
        return await context.Screens.ToListAsync();
    }

    // Get screens belonging to a particular theatre
    public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
    {
        return await context.Screens
            .Where(s => s.TheatreId == theatreId)
            .ToListAsync();
    }

    // Get screen by ID
    public async Task<Screen?> GetByIdAsync(int id)
    {
        return await context.Screens
            .FirstOrDefaultAsync(s => s.ScreenId == id);
    }

    // Add a new screen
    public async Task<Screen> AddAsync(Screen screen)
    {
        await context.Screens.AddAsync(screen);

        await context.SaveChangesAsync();

        return screen;
    }

    // Update an existing screen
    public async Task<Screen> UpdateAsync(Screen screen)
    {
        context.Screens.Update(screen);

        await context.SaveChangesAsync();

        return screen;
    }

    // Delete a screen
    public async Task<bool> DeleteAsync(int id)
    {
        var screen = await context.Screens.FindAsync(id);

        if (screen == null)
        {
            return false;
        }

        context.Screens.Remove(screen);

        await context.SaveChangesAsync();

        return true;
    }
}
