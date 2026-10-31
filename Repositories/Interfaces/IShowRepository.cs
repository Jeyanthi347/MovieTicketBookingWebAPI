using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface IShowRepository
{
    // Get all shows
    Task<List<Show>> GetAllAsync();

    // Get show by ID
    Task<Show?> GetByIdAsync(int id);

    // Add a new show
    Task<Show> AddAsync(Show show);

    // Update an existing show
    Task<Show> UpdateAsync(Show show);

    // Delete a show
    Task<bool> DeleteAsync(int id);
}