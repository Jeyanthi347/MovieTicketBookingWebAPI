using MovieTicketBookingWebAPI.DTOs.Bookings;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IBookingService
{
    Task<List<BookingResponseDto>> GetAllAsync(
        int? userId = null);

    Task<BookingDetailsResponseDto?> GetByIdAsync(
        int id,
        int? userId = null);

    Task<List<BookingResponseDto>> GetByShowIdAsync(
        int showId,
        int? userId = null);

    Task<BookingResponseDto> CreateAsync(
        CreateBookingDto dto,
        int userId);

    Task<bool> CancelAsync(
        int id,
        int? userId = null);
}