
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Seats;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface ISeatService
{
    // Get paginated seats belonging to a screen
    Task<PaginationResponseDto<SeatResponseDto>> GetByScreenIdAsync(
        int screenId,
        int pageNumber,
        int pageSize);

    // Get a seat by ID
    Task<SeatResponseDto?> GetByIdAsync(int id);

    // Create one seat
    Task<SeatResponseDto> CreateAsync(
        CreateSeatDto dto,
        int? managerUserId = null);

    // Create multiple seats
    Task<List<SeatResponseDto>> BulkCreateAsync(
        BulkCreateSeatsDto dto,
        int? managerUserId = null);

    // Update a seat
    Task<SeatResponseDto?> UpdateAsync(
        int id,
        UpdateSeatDto dto,
        int? managerUserId = null);

    // Delete a seat
    Task<bool> DeleteAsync(
        int id,
        int? managerUserId = null);
}
