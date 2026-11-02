using MovieTicketBookingWebAPI.DTOs;
using MovieTicketBookingWebAPI.DTOs.Screens;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository repository;

    public ScreenService(IScreenRepository repository)
    {
        this.repository = repository;
    }

    // Get all screens
    public async Task<List<ScreenResponseDto>> GetAllAsync()
    {
        var screens = await repository.GetAllAsync();

        return screens.Select(s => new ScreenResponseDto
        {
            ScreenId = s.ScreenId,
            TheatreId = s.TheatreId,
            ScreenName = s.ScreenName
        }).ToList();
    }

    // Get screen by ID
    public async Task<ScreenResponseDto?> GetByIdAsync(int id)
    {
        var screen = await repository.GetByIdAsync(id);

        if (screen == null)
        {
            return null;
        }

        return new ScreenResponseDto
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            ScreenName = screen.ScreenName
        };
    }

    // Create a new screen
    public async Task<ScreenResponseDto> CreateAsync(CreateScreenDto dto)
    {
        var screen = new Screen
        {
            TheatreId = dto.TheatreId,
            ScreenName = dto.ScreenName
        };

        var createdScreen = await repository.AddAsync(screen);

        return new ScreenResponseDto
        {
            ScreenId = createdScreen.ScreenId,
            TheatreId = createdScreen.TheatreId,
            ScreenName = createdScreen.ScreenName
        };
    }

    // Update an existing screen
    public async Task<ScreenResponseDto?> UpdateAsync(int id, UpdateScreenDto dto)
    {
        var screen = await repository.GetByIdAsync(id);

        if (screen == null)
        {
            return null;
        }

        screen.TheatreId = dto.TheatreId;
        screen.ScreenName = dto.ScreenName;

        var updatedScreen = await repository.UpdateAsync(screen);

        return new ScreenResponseDto
        {
            ScreenId = updatedScreen.ScreenId,
            TheatreId = updatedScreen.TheatreId,
            ScreenName = updatedScreen.ScreenName
        };
    }

    // Delete a screen
    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }
}