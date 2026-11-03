
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface ITheatreRepository
{
    Task<(List<Theatre> Theatres, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize);

    Task<Theatre?> GetByIdAsync(int id);

    Task<Theatre?> GetByManagerUserIdAsync(int userId);

    Task<Theatre> AddAsync(Theatre theatre);

    Task<Theatre> UpdateAsync(Theatre theatre);

    Task<bool> DeleteAsync(int id);
}