
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;

namespace MovieTicketBookingWebAPI.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext context;

    public MovieRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<(List<Movie> Movies, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize)
    {
        int totalRecords = await context.Movies.CountAsync();

        var movies = await context.Movies
            .OrderBy(m => m.MovieId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (movies, totalRecords);
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await context.Movies.FindAsync(id);
    }

    public async Task<Movie> AddAsync(Movie movie)
    {
        await context.Movies.AddAsync(movie);

        await context.SaveChangesAsync();

        return movie;
    }

    public async Task<Movie?> UpdateAsync(int id, Movie movie)
    {
        var existingMovie = await context.Movies.FindAsync(id);

        if (existingMovie == null)
        {
            return null;
        }

        existingMovie.MovieName = movie.MovieName;
        existingMovie.Genre = movie.Genre;
        existingMovie.DurationMinutes = movie.DurationMinutes;
        existingMovie.Description = movie.Description;
        existingMovie.Language = movie.Language;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.Rating = movie.Rating;
        existingMovie.IsActive = movie.IsActive;

        await context.SaveChangesAsync();

        return existingMovie;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await context.Movies.FindAsync(id);

        if (movie == null)
        {
            return false;
        }

        context.Movies.Remove(movie);

        await context.SaveChangesAsync();

        return true;
    }
}
