using MovieTicketBookingWebAPI.DTOs.Theatres;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface ITheatreService
{
    Task<List<TheatreResponseDto>> GetAllAsync();

    Task<TheatreResponseDto?> GetByIdAsync(int id);

    Task<TheatreResponseDto> CreateAsync(CreateTheatreDto dto);

    Task<TheatreResponseDto?> UpdateAsync(int id, UpdateTheatreDto dto);

    Task<bool> DeleteAsync(int id);
}