using MovieTicketBookingWebAPI.DTOs.Shows;

namespace MovieTicketBookingWebAPI.Services.Interfaces;

public interface IShowService
{
    // Get all shows
    Task<List<ShowResponseDto>> GetAllAsync();

    // Get show by ID
    Task<ShowResponseDto?> GetByIdAsync(int id);

    // Create a new show
    Task<ShowResponseDto> CreateAsync(CreateShowDto dto);

    // Get seat layout for a particular show
    Task<List<ShowSeatResponseDto>?> GetSeatLayoutAsync(int showId);

    // Update an existing show
    Task<ShowResponseDto?> UpdateAsync(int id, UpdateShowDto dto);

    // Delete a show
    Task<bool> DeleteAsync(int id);
}