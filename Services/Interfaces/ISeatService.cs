using MovieTicketBookingWebAPI.DTOs;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface ISeatService
{
    // Get all seats belonging to a particular screen
    Task<List<SeatResponseDto>> GetByScreenIdAsync(int screenId);

    // Get a seat by ID
    Task<SeatResponseDto?> GetByIdAsync(int id);

    // Create one seat
    Task<SeatResponseDto> CreateAsync(CreateSeatDto dto);

    // Create multiple seats
    Task<List<SeatResponseDto>> BulkCreateAsync(BulkCreateSeatsDto dto);

    // Update a seat
    Task<SeatResponseDto?> UpdateAsync(int id, UpdateSeatDto dto);

    // Delete a seat
    Task<bool> DeleteAsync(int id);
}