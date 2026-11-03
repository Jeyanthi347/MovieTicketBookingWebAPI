
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface ISeatRepository
{
    Task<(List<Seat> Seats, int TotalRecords)> GetByScreenIdAsync(
        int screenId,
        int pageNumber,
        int pageSize);

    Task<Seat?> GetByIdAsync(int id);

    Task<Seat> AddAsync(Seat seat);

    Task<List<Seat>> AddRangeAsync(List<Seat> seats);

    Task<Seat> UpdateAsync(Seat seat);

    Task<bool> DeleteAsync(int id);
}
