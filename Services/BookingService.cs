
using MovieTicketBookingWebAPI.DTOs.Bookings;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository repository;
    private readonly IUserRepository userRepository;

    public BookingService(
        IBookingRepository repository,
        IUserRepository userRepository)
    {
        this.repository = repository;
        this.userRepository = userRepository;
    }

    public async Task<List<BookingResponseDto>> GetAllAsync(
        int? userId = null)
    {
        var bookings = await repository.GetAllAsync(userId);
        var result = new List<BookingResponseDto>();

        foreach (var booking in bookings)
        {
            var seats = await repository.GetSeatLabelsAsync(
                booking.BookingId);

            result.Add(new BookingResponseDto
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                ShowId = booking.ShowId,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                Seats = seats
            });
        }

        return result;
    }

    public async Task<BookingDetailsResponseDto?> GetByIdAsync(
        int id, int? userId = null)
    {
        var booking = await repository.GetByIdAsync(id);

        if (booking == null)
            return null;

        if (userId != null && booking.UserId != userId)
            return null;

        var seats = await repository.GetSeatLabelsAsync(
            booking.BookingId);

        return new BookingDetailsResponseDto
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            CustomerName = booking.CustomerName,
            CustomerEmail = booking.CustomerEmail,
            CustomerPhone = booking.CustomerPhone,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            Seats = seats
        };
    }

    public async Task<List<BookingResponseDto>> GetByShowIdAsync(
        int showId, int? userId = null)
    {
        var bookings = await repository.GetByShowIdAsync(
            showId, userId);

        var result = new List<BookingResponseDto>();

        foreach (var booking in bookings)
        {
            var seats = await repository.GetSeatLabelsAsync(
                booking.BookingId);

            result.Add(new BookingResponseDto
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                ShowId = booking.ShowId,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                Seats = seats
            });
        }

        return result;
    }

    public async Task<BookingResponseDto> CreateAsync(
        CreateBookingDto dto, int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (string.IsNullOrWhiteSpace(dto.CustomerPhone))
            throw new ArgumentException("Customer phone is required.");

        if (dto.SeatIds == null || dto.SeatIds.Count == 0)
            throw new ArgumentException("Please select at least one seat.");

        if (dto.SeatIds.Distinct().Count() != dto.SeatIds.Count)
            throw new ArgumentException("Duplicate seat IDs are not allowed.");

        var show = await repository.GetShowByIdAsync(dto.ShowId);

        if (show == null)
            throw new KeyNotFoundException("Show not found.");

        var showSeats = await repository.GetShowSeatsByIdsAsync(
            dto.ShowId, dto.SeatIds);

        if (showSeats.Count != dto.SeatIds.Count)
            throw new ArgumentException(
                "One or more selected seats do not belong to this show.");

        if (showSeats.Any(ss => ss.Status != "Available"))
            throw new InvalidOperationException(
                "One or more selected seats are already booked.");

        decimal totalAmount =
            show.TicketPrice * dto.SeatIds.Count;

        var booking = new Booking
        {
            UserId = user.UserId,
            ShowId = dto.ShowId,
            CustomerName = user.FullName,
            CustomerEmail = user.Email,
            CustomerPhone = dto.CustomerPhone,
            BookingDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = "Confirmed"
        };

        var bookingSeats = new List<BookingSeat>();

        foreach (var showSeat in showSeats)
        {
            bookingSeats.Add(new BookingSeat
            {
                SeatId = showSeat.SeatId,
                Price = show.TicketPrice
            });
        }

        var savedBooking = await repository.CreateAsync(
            booking, bookingSeats, showSeats);

        var seatLabels = await repository.GetSeatLabelsAsync(
            savedBooking.BookingId);

        return new BookingResponseDto
        {
            BookingId = savedBooking.BookingId,
            BookingReference = savedBooking.BookingReference,
            ShowId = savedBooking.ShowId,
            TotalAmount = savedBooking.TotalAmount,
            Status = savedBooking.Status,
            Seats = seatLabels
        };
    }

    public async Task<bool> CancelAsync(
        int id, int? userId = null)
    {
        var booking = await repository.GetByIdAsync(id);

        if (booking == null)
            return false;

        if (userId != null && booking.UserId != userId)
            return false;

        if (booking.Status != "Confirmed")
            throw new InvalidOperationException(
                "Only confirmed bookings can be cancelled.");

        var showSeats =
            await repository.GetShowSeatsForBookingAsync(booking);

        await repository.CancelAsync(booking, showSeats);

        return true;
    }
}
