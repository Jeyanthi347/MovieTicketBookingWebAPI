using MovieTicketBookingWebAPI.DTOs;
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface IShowRepository
{
    // Get all shows
    Task<List<Show>> GetAllAsync();

    // Get show by ID
    Task<Show?> GetByIdAsync(int id);

    // Add a new show and automatically generate its ShowSeats
    Task<Show> AddAsync(Show show);

    // Get the seat layout for a particular show
    Task<List<ShowSeatResponseDto>> GetSeatLayoutAsync(int showId);

    // Update an existing show
    Task<Show> UpdateAsync(Show show);

    // Delete a show
    Task<bool> DeleteAsync(int id);
}