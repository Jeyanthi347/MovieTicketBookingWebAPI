using MovieTicketBookingWebAPI.DTOs;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository repository;

    public ShowService(IShowRepository repository)
    {
        this.repository = repository;
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
    public async Task<ShowResponseDto> CreateAsync(CreateShowDto dto)
    {
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
        // First check whether the show exists
        var show = await repository.GetByIdAsync(showId);

        if (show == null)
            return null;

        // Get all seats and their availability for this show
        return await repository.GetSeatLayoutAsync(showId);
    }

    // Update an existing show
    public async Task<ShowResponseDto?> UpdateAsync(int id, UpdateShowDto dto)
    {
        if (dto.EndTime <= dto.StartTime)
            throw new ArgumentException("End time must be later than start time.");

        if (dto.TicketPrice < 0)
            throw new ArgumentException("Ticket price cannot be negative.");

        var show = await repository.GetByIdAsync(id);

        if (show == null)
            return null;

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
    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }
}