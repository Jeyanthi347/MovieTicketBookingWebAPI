using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<List<Screen>> GetAllAsync();

    Task<Screen?> GetByIdAsync(int id);

    Task<Screen> AddAsync(Screen screen);

    Task<Screen> UpdateAsync(Screen screen);

    Task<bool> DeleteAsync(int id);
}