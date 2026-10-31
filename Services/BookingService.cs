using MovieTicketBookingWebAPI.DTOs;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository repository;

    public BookingService(IBookingRepository repository)
    {
        this.repository = repository;
    }

    // Get all bookings
    public async Task<List<BookingResponseDto>> GetAllAsync()
    {
        var bookings = await repository.GetAllAsync();

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

    // Get booking by ID
    public async Task<BookingDetailsResponseDto?> GetByIdAsync(int id)
    {
        var booking = await repository.GetByIdAsync(id);

        if (booking == null)
        {
            return null;
        }

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

    // Get bookings for a particular show
    public async Task<List<BookingResponseDto>> GetByShowIdAsync(
        int showId)
    {
        var bookings = await repository.GetByShowIdAsync(showId);

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

    // Create a booking
    public async Task<BookingResponseDto> CreateAsync(
        CreateBookingDto dto)
    {
        // Validate customer details
        if (string.IsNullOrWhiteSpace(dto.CustomerName) ||
            string.IsNullOrWhiteSpace(dto.CustomerEmail) ||
            string.IsNullOrWhiteSpace(dto.CustomerPhone))
        {
            throw new ArgumentException(
                "Customer details are required.");
        }

        // Validate selected seats
        if (dto.SeatIds == null || dto.SeatIds.Count == 0)
        {
            throw new ArgumentException(
                "Please select at least one seat.");
        }

        // Check duplicate seat IDs
        if (dto.SeatIds.Distinct().Count() != dto.SeatIds.Count)
        {
            throw new ArgumentException(
                "Duplicate seat IDs are not allowed.");
        }

        // Check whether the show exists
        var show = await repository.GetShowByIdAsync(dto.ShowId);

        if (show == null)
        {
            throw new KeyNotFoundException("Show not found.");
        }

        // Get selected seats for this show
        var showSeats = await repository.GetShowSeatsByIdsAsync(
            dto.ShowId, dto.SeatIds);

        // Check whether all requested seats belong to this show
        if (showSeats.Count != dto.SeatIds.Count)
        {
            throw new ArgumentException(
                "One or more selected seats do not belong to this show.");
        }

        // Check whether all selected seats are available
        if (showSeats.Any(ss => ss.Status != "Available"))
        {
            throw new InvalidOperationException(
                "One or more selected seats are already booked.");
        }

        // Calculate total amount on the server
        decimal totalAmount = show.TicketPrice * dto.SeatIds.Count;

        // Create booking object
        var booking = new Booking
        {
            ShowId = dto.ShowId,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            CustomerPhone = dto.CustomerPhone,
            BookingDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = "Confirmed"
        };

        // Create BookingSeat records
        var bookingSeats = new List<BookingSeat>();

        foreach (var showSeat in showSeats)
        {
            bookingSeats.Add(new BookingSeat
            {
                SeatId = showSeat.SeatId,
                Price = show.TicketPrice
            });
        }

        // Save booking, booking seats and update seat statuses
        var savedBooking = await repository.CreateAsync(
            booking, bookingSeats, showSeats);

        // Get seat labels for the response
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

    // Cancel a booking
    public async Task<bool> CancelAsync(int id)
    {
        var booking = await repository.GetByIdAsync(id);

        if (booking == null)
        {
            return false;
        }

        if (booking.Status != "Confirmed")
        {
            throw new InvalidOperationException(
                "Only confirmed bookings can be cancelled.");
        }

        // Find the seats belonging to this booking
        var showSeats = await repository.GetShowSeatsForBookingAsync(
            booking);

        // Cancel booking and release seats
        await repository.CancelAsync(booking, showSeats);

        return true;
    }
}