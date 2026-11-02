
using MovieTicketBookingWebAPI.DTOs.Shows;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository repository;
    private readonly IScreenRepository screenRepository;
    private readonly ITheatreRepository theatreRepository;

    public ShowService(
        IShowRepository repository,
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

    // Get all shows
    public async Task<List<ShowResponseDto>> GetAllAsync()
    {
        var shows = await repository.GetAllAsync();

        return shows.Select(s => new ShowResponseDto
        {
            ShowId = s.ShowId,
            MovieId = s.MovieId,
            ScreenId = s.ScreenId,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            TicketPrice = s.TicketPrice
        }).ToList();
    }

    // Get show by ID
    public async Task<ShowResponseDto?> GetByIdAsync(int id)
    {
        var show = await repository.GetByIdAsync(id);

        if (show == null)
            return null;

        return new ShowResponseDto
        {
            ShowId = show.ShowId,
            MovieId = show.MovieId,
            ScreenId = show.ScreenId,
            StartTime = show.StartTime,
            EndTime = show.EndTime,
            TicketPrice = show.TicketPrice
        };
    }

    // Create a new show
    public async Task<ShowResponseDto> CreateAsync(
        CreateShowDto dto,
        int? managerUserId = null)
    {
        if (managerUserId.HasValue &&
            !await IsScreenAssignedToManagerAsync(
                dto.ScreenId, managerUserId.Value))
        {
            throw new UnauthorizedAccessException();
        }

        if (dto.EndTime <= dto.StartTime)
            throw new ArgumentException("End time must be later than start time.");

        if (dto.TicketPrice < 0)
            throw new ArgumentException("Ticket price cannot be negative.");

        var show = new Show
        {
            MovieId = dto.MovieId,
            ScreenId = dto.ScreenId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            TicketPrice = dto.TicketPrice
        };

        var createdShow = await repository.AddAsync(show);

        return new ShowResponseDto
        {
            ShowId = createdShow.ShowId,
            MovieId = createdShow.MovieId,
            ScreenId = createdShow.ScreenId,
            StartTime = createdShow.StartTime,
            EndTime = createdShow.EndTime,
            TicketPrice = createdShow.TicketPrice
        };
    }

    // Get seat layout for a particular show
    public async Task<List<ShowSeatResponseDto>?> GetSeatLayoutAsync(int showId)
    {
        var show = await repository.GetByIdAsync(showId);

        if (show == null)
            return null;

        return await repository.GetSeatLayoutAsync(showId);
    }

    // Update an existing show
    public async Task<ShowResponseDto?> UpdateAsync(
        int id,
        UpdateShowDto dto,
        int? managerUserId = null)
    {
        if (dto.EndTime <= dto.StartTime)
            throw new ArgumentException("End time must be later than start time.");

        if (dto.TicketPrice < 0)
            throw new ArgumentException("Ticket price cannot be negative.");

        var show = await repository.GetByIdAsync(id);

        if (show == null)
            return null;

        if (managerUserId.HasValue)
        {
            bool existingScreenAllowed =
                await IsScreenAssignedToManagerAsync(
                    show.ScreenId, managerUserId.Value);

            bool newScreenAllowed =
                await IsScreenAssignedToManagerAsync(
                    dto.ScreenId, managerUserId.Value);

            if (!existingScreenAllowed || !newScreenAllowed)
                throw new UnauthorizedAccessException();
        }

        show.MovieId = dto.MovieId;
        show.ScreenId = dto.ScreenId;
        show.StartTime = dto.StartTime;
        show.EndTime = dto.EndTime;
        show.TicketPrice = dto.TicketPrice;

        var updatedShow = await repository.UpdateAsync(show);

        return new ShowResponseDto
        {
            ShowId = updatedShow.ShowId,
            MovieId = updatedShow.MovieId,
            ScreenId = updatedShow.ScreenId,
            StartTime = updatedShow.StartTime,
            EndTime = updatedShow.EndTime,
            TicketPrice = updatedShow.TicketPrice
        };
    }

    // Delete a show
    public async Task<bool> DeleteAsync(
        int id,
        int? managerUserId = null)
    {
        var show = await repository.GetByIdAsync(id);

        if (show == null)
            return false;

        if (managerUserId.HasValue &&
            !await IsScreenAssignedToManagerAsync(
                show.ScreenId, managerUserId.Value))
        {
            throw new UnauthorizedAccessException();
        }

        return await repository.DeleteAsync(id);
    }
}
