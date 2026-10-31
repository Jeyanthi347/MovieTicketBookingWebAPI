
using MovieTicketBookingWebAPI.DTOs;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository;
    }

    // Get all movies
    public async Task<List<MovieResponseDto>> GetAllAsync()
    {
        var movies = await movieRepository.GetAllAsync();

        return movies.Select(movie => new MovieResponseDto
        {
            MovieId = movie.MovieId,
            MovieName = movie.MovieName,
            Genre = movie.Genre,
            DurationMinutes = movie.DurationMinutes,
            Description = movie.Description,
            Language = movie.Language,
            ReleaseDate = movie.ReleaseDate,
            IsActive = movie.IsActive
        }).ToList();
    }

    // Get movie by ID
    public async Task<MovieResponseDto?> GetByIdAsync(int id)
    {
        var movie = await movieRepository.GetByIdAsync(id);

        if (movie == null)
        {
            return null;
        }

        return new MovieResponseDto
        {
            MovieId = movie.MovieId,
            MovieName = movie.MovieName,
            Genre = movie.Genre,
            DurationMinutes = movie.DurationMinutes,
            Description = movie.Description,
            Language = movie.Language,
            ReleaseDate = movie.ReleaseDate,
            IsActive = movie.IsActive
        };
    }

    // Create movie
    public async Task<MovieResponseDto> CreateAsync(CreateMovieDto dto)
    {
        var movie = new Movie
        {
            MovieName = dto.MovieName,
            Genre = dto.Genre,
            DurationMinutes = dto.DurationMinutes,
            Description = dto.Description,
            Language = dto.Language,
            ReleaseDate = dto.ReleaseDate,
            IsActive = true
        };

        var createdMovie = await movieRepository.AddAsync(movie);

        return new MovieResponseDto
        {
            MovieId = createdMovie.MovieId,
            MovieName = createdMovie.MovieName,
            Genre = createdMovie.Genre,
            DurationMinutes = createdMovie.DurationMinutes,
            Description = createdMovie.Description,
            Language = createdMovie.Language,
            ReleaseDate = createdMovie.ReleaseDate,
            IsActive = createdMovie.IsActive
        };
    }

    // Update movie
    public async Task<MovieResponseDto?> UpdateAsync(int id, UpdateMovieDto dto)
    {
        var movie = new Movie
        {
            MovieName = dto.MovieName,
            Genre = dto.Genre,
            DurationMinutes = dto.DurationMinutes,
            Description = dto.Description,
            Language = dto.Language,
            ReleaseDate = dto.ReleaseDate,
            IsActive = dto.IsActive
        };

        var updatedMovie = await movieRepository.UpdateAsync(id, movie);

        if (updatedMovie == null)
        {
            return null;
        }

        return new MovieResponseDto
        {
            MovieId = updatedMovie.MovieId,
            MovieName = updatedMovie.MovieName,
            Genre = updatedMovie.Genre,
            DurationMinutes = updatedMovie.DurationMinutes,
            Description = updatedMovie.Description,
            Language = updatedMovie.Language,
            ReleaseDate = updatedMovie.ReleaseDate,
            IsActive = updatedMovie.IsActive
        };
    }

    // Delete movie
    public async Task<bool> DeleteAsync(int id)
    {
        return await movieRepository.DeleteAsync(id);
    }
}
