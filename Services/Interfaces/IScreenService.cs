
using MovieTicketBookingWebAPI.DTOs.Screens;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IScreenService
{
    Task<List<ScreenResponseDto>> GetAllAsync();

    Task<ScreenResponseDto?> GetByIdAsync(int id);

    Task<ScreenResponseDto> CreateAsync(
        CreateScreenDto dto,
        int? managerUserId = null);

    Task<ScreenResponseDto?> UpdateAsync(
        int id,
        UpdateScreenDto dto,
        int? managerUserId = null);

    Task<bool> DeleteAsync(
        int id,
        int? managerUserId = null);
}
