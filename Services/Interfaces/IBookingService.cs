using MovieTicketBookingWebAPI.DTOs;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IBookingService
{
    Task<List<BookingResponseDto>> GetAllAsync();

    Task<BookingDetailsResponseDto?> GetByIdAsync(int id);

    Task<List<BookingResponseDto>> GetByShowIdAsync(int showId);

    Task<BookingResponseDto> CreateAsync(CreateBookingDto dto);

    Task<bool> CancelAsync(int id);
}