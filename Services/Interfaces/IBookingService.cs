
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Bookings;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IBookingService
{
    Task<PaginationResponseDto<BookingResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        int? userId = null);

    Task<BookingDetailsResponseDto?> GetByIdAsync(
        int id,
        int? userId = null);

    Task<PaginationResponseDto<BookingResponseDto>> GetByShowIdAsync(
        int showId,
        int pageNumber,
        int pageSize,
        int? userId = null);

    Task<BookingResponseDto> CreateAsync(
        CreateBookingDto dto,
        int userId);

    Task<bool> CancelAsync(
        int id,
        int? userId = null);
}
