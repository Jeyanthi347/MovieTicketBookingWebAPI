
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Theatres;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface ITheatreService
{
    Task<PaginationResponseDto<TheatreResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize);

    Task<TheatreResponseDto?> GetByIdAsync(int id);

    Task<TheatreResponseDto> CreateAsync(CreateTheatreDto dto);

    Task<TheatreResponseDto?> UpdateAsync(int id, UpdateTheatreDto dto);

    Task<bool> DeleteAsync(int id);
}
