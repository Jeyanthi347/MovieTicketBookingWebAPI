
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<Show?> GetShowByIdAsync(int showId);

    Task<List<ShowSeat>> GetShowSeatsByIdsAsync(
        int showId,
        List<int> seatIds);

    Task<(List<Booking> Bookings, int TotalRecords)> GetAllAsync(
        int pageNumber,
        int pageSize,
        int? userId = null);

    Task<Booking?> GetByIdAsync(int id);

    Task<(List<Booking> Bookings, int TotalRecords)> GetByShowIdAsync(
        int showId,
        int pageNumber,
        int pageSize,
        int? userId = null);

    Task<List<string>> GetSeatLabelsAsync(int bookingId);

    Task<List<ShowSeat>> GetShowSeatsForBookingAsync(
        Booking booking);

    Task<Booking> CreateAsync(
        Booking booking,
        List<BookingSeat> bookingSeats,
        List<ShowSeat> showSeats);

    Task CancelAsync(
        Booking booking,
        List<ShowSeat> showSeats);
}
