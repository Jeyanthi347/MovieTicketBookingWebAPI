using MovieTicketBookingWebAPI.DTOs;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IShowService
{
    // Get all shows
    Task<List<ShowResponseDto>> GetAllAsync();

    // Get show by ID
    Task<ShowResponseDto?> GetByIdAsync(int id);

    // Create a new show
    Task<ShowResponseDto> CreateAsync(CreateShowDto dto);

    // Update an existing show
    Task<ShowResponseDto?> UpdateAsync(int id, UpdateShowDto dto);

    // Delete a show
    Task<bool> DeleteAsync(int id);
}