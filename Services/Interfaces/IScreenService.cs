using MovieTicketBookingWebAPI.DTOs;
using MovieTicketBookingWebAPI.DTOs.Screens;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IScreenService
{
    Task<List<ScreenResponseDto>> GetAllAsync();

    Task<ScreenResponseDto?> GetByIdAsync(int id);

    Task<ScreenResponseDto> CreateAsync(CreateScreenDto dto);

    Task<ScreenResponseDto?> UpdateAsync(int id, UpdateScreenDto dto);

    Task<bool> DeleteAsync(int id);
}