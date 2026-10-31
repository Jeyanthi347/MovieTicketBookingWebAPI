
using MovieTicketBookingWebAPI.DTOs;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IMovieService
{
    Task<List<MovieResponseDto>> GetAllAsync();

    Task<MovieResponseDto?> GetByIdAsync(int id);

    Task<MovieResponseDto> CreateAsync(CreateMovieDto dto);

    Task<MovieResponseDto?> UpdateAsync(int id, UpdateMovieDto dto);

    Task<bool> DeleteAsync(int id);
}
