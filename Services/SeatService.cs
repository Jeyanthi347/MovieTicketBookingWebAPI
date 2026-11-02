
using MovieTicketBookingWebAPI.DTOs.Seats;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository repository;
    private readonly IScreenRepository screenRepository;
    private readonly ITheatreRepository theatreRepository;

    public SeatService(
        ISeatRepository repository,
        IScreenRepository screenRepository,
        ITheatreRepository theatreRepository)
    {
        this.repository = repository;
        this.screenRepository = screenRepository;
        this.theatreRepository = theatreRepository;
    }

    // Check whether a screen belongs to the manager's assigned theatre
    private async Task<bool> IsScreenAssignedToManagerAsync(
        int screenId,
        int managerUserId)
    {
        var theatre = await theatreRepository
            .GetByManagerUserIdAsync(managerUserId);

        if (theatre == null)
            return false;

        var screens = await screenRepository
            .GetByTheatreIdAsync(theatre.TheatreId);

        return screens.Any(s => s.ScreenId == screenId);
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
    public async Task<SeatResponseDto> CreateAsync(
        CreateSeatDto dto,
        int? managerUserId = null)
    {
        if (managerUserId.HasValue &&
            !await IsScreenAssignedToManagerAsync(
                dto.ScreenId, managerUserId.Value))
        {
            throw new UnauthorizedAccessException();
        }

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
    public async Task<List<SeatResponseDto>> BulkCreateAsync(
        BulkCreateSeatsDto dto,
        int? managerUserId = null)
    {
        if (managerUserId.HasValue &&
            !await IsScreenAssignedToManagerAsync(
                dto.ScreenId, managerUserId.Value))
        {
            throw new UnauthorizedAccessException();
        }

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
    public async Task<SeatResponseDto?> UpdateAsync(
        int id,
        UpdateSeatDto dto,
        int? managerUserId = null)
    {
        var seat = await repository.GetByIdAsync(id);

        if (seat == null)
            return null;

        if (managerUserId.HasValue)
        {
            bool existingScreenAllowed =
                await IsScreenAssignedToManagerAsync(
                    seat.ScreenId, managerUserId.Value);

            bool newScreenAllowed =
                await IsScreenAssignedToManagerAsync(
                    dto.ScreenId, managerUserId.Value);

            if (!existingScreenAllowed || !newScreenAllowed)
                throw new UnauthorizedAccessException();
        }

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
    public async Task<bool> DeleteAsync(
        int id,
        int? managerUserId = null)
    {
        var seat = await repository.GetByIdAsync(id);

        if (seat == null)
            return false;

        if (managerUserId.HasValue &&
            !await IsScreenAssignedToManagerAsync(
                seat.ScreenId, managerUserId.Value))
        {
            throw new UnauthorizedAccessException();
        }

        return await repository.DeleteAsync(id);
    }
}
