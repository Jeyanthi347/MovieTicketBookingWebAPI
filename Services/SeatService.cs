using MovieTicketBookingWebAPI.DTOs.Seats;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository repository;

    public SeatService(ISeatRepository repository)
    {
        this.repository = repository;
    }

    // Get all seats belonging to a screen
    public async Task<List<SeatResponseDto>> GetByScreenIdAsync(int screenId)
    {
        var seats = await repository.GetByScreenIdAsync(screenId);

        return seats.Select(s => new SeatResponseDto
        {
            SeatId = s.SeatId,
            ScreenId = s.ScreenId,
            RowName = s.RowName,
            SeatNumber = s.SeatNumber,
            SeatType = s.SeatType
        }).ToList();
    }

    // Get seat by ID
    public async Task<SeatResponseDto?> GetByIdAsync(int id)
    {
        var seat = await repository.GetByIdAsync(id);

        if (seat == null)
            return null;

        return new SeatResponseDto
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }

    // Create one seat
    public async Task<SeatResponseDto> CreateAsync(CreateSeatDto dto)
    {
        var seat = new Seat
        {
            ScreenId = dto.ScreenId,
            RowName = dto.RowName,
            SeatNumber = dto.SeatNumber,
            SeatType = dto.SeatType
        };

        var createdSeat = await repository.AddAsync(seat);

        return new SeatResponseDto
        {
            SeatId = createdSeat.SeatId,
            ScreenId = createdSeat.ScreenId,
            RowName = createdSeat.RowName,
            SeatNumber = createdSeat.SeatNumber,
            SeatType = createdSeat.SeatType
        };
    }

    // Create multiple seats
    public async Task<List<SeatResponseDto>> BulkCreateAsync(BulkCreateSeatsDto dto)
    {
        var seats = dto.Seats.Select(s => new Seat
        {
            ScreenId = dto.ScreenId,
            RowName = s.RowName,
            SeatNumber = s.SeatNumber,
            SeatType = s.SeatType
        }).ToList();

        var createdSeats = await repository.AddRangeAsync(seats);

        return createdSeats.Select(s => new SeatResponseDto
        {
            SeatId = s.SeatId,
            ScreenId = s.ScreenId,
            RowName = s.RowName,
            SeatNumber = s.SeatNumber,
            SeatType = s.SeatType
        }).ToList();
    }

    // Update seat
    public async Task<SeatResponseDto?> UpdateAsync(int id, UpdateSeatDto dto)
    {
        var seat = await repository.GetByIdAsync(id);

        if (seat == null)
            return null;

        seat.ScreenId = dto.ScreenId;
        seat.RowName = dto.RowName;
        seat.SeatNumber = dto.SeatNumber;
        seat.SeatType = dto.SeatType;

        var updatedSeat = await repository.UpdateAsync(seat);

        return new SeatResponseDto
        {
            SeatId = updatedSeat.SeatId,
            ScreenId = updatedSeat.ScreenId,
            RowName = updatedSeat.RowName,
            SeatNumber = updatedSeat.SeatNumber,
            SeatType = updatedSeat.SeatType
        };
    }

    // Delete seat
    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }
}