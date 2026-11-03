
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<(List<Screen> Screens, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize);

    Task<List<Screen>> GetByTheatreIdAsync(int theatreId);

    Task<Screen?> GetByIdAsync(int id);

    Task<Screen> AddAsync(Screen screen);

    Task<Screen> UpdateAsync(Screen screen);

    Task<bool> DeleteAsync(int id);
}
