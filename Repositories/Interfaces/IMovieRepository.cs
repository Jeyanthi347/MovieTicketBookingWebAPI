using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<(List<Movie> Movies, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize);

    Task<Movie?> GetByIdAsync(int id);

    Task<Movie> AddAsync(Movie movie);

    Task<Movie?> UpdateAsync(int id, Movie movie);

    Task<bool> DeleteAsync(int id);
}