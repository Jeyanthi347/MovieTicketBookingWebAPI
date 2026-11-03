
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Movies;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IMovieService
{
    Task<PaginationResponseDto<MovieResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize);

    Task<MovieResponseDto?> GetByIdAsync(int id);

    Task<MovieResponseDto> CreateAsync(CreateMovieDto dto);

    Task<MovieResponseDto?> UpdateAsync(int id, UpdateMovieDto dto);

    Task<bool> DeleteAsync(int id);
}
