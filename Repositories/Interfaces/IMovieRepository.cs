
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();

    Task<Movie?> GetByIdAsync(int id);

    Task<Movie> AddAsync(Movie movie);

    Task<Movie?> UpdateAsync(int id, Movie movie);

    Task<bool> DeleteAsync(int id);
}
