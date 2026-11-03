
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Screens;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository repository;
    private readonly ITheatreRepository theatreRepository;

    public ScreenService(
        IScreenRepository repository,
        ITheatreRepository theatreRepository)
    {
        this.repository = repository;
        this.theatreRepository = theatreRepository;
    }

    // Get paginated screens
    public async Task<PaginationResponseDto<ScreenResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize)
    {
        var result = await repository.GetAllAsync(pageNumber, pageSize);

        var screens = result.Screens.Select(s => new ScreenResponseDto
        {
            ScreenId = s.ScreenId,
            TheatreId = s.TheatreId,
            ScreenName = s.ScreenName
        }).ToList();

        return new PaginationResponseDto<ScreenResponseDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = result.TotalRecords,
            TotalPages = (int)Math.Ceiling(
                result.TotalRecords / (double)pageSize),
            Data = screens
        };
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
    public async Task<ScreenResponseDto> CreateAsync(
        CreateScreenDto dto,
        int? managerUserId = null)
    {
        // Check theatre ownership for TheatreManager
        if (managerUserId.HasValue)
        {
            var assignedTheatre =
                await theatreRepository.GetByManagerUserIdAsync(managerUserId.Value);

            if (assignedTheatre == null ||
                assignedTheatre.TheatreId != dto.TheatreId)
            {
                throw new UnauthorizedAccessException(
                    "You are not assigned to this theatre.");
            }
        }

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
    public async Task<ScreenResponseDto?> UpdateAsync(
        int id,
        UpdateScreenDto dto,
        int? managerUserId = null)
    {
        var screen = await repository.GetByIdAsync(id);

        if (screen == null)
        {
            return null;
        }

        // Check theatre ownership for TheatreManager
        if (managerUserId.HasValue)
        {
            var assignedTheatre =
                await theatreRepository.GetByManagerUserIdAsync(managerUserId.Value);

            if (assignedTheatre == null ||
                screen.TheatreId != assignedTheatre.TheatreId ||
                dto.TheatreId != assignedTheatre.TheatreId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to update screens outside your assigned theatre.");
            }
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
    public async Task<bool> DeleteAsync(
        int id,
        int? managerUserId = null)
    {
        if (managerUserId.HasValue)
        {
            var screen = await repository.GetByIdAsync(id);

            if (screen == null)
            {
                return false;
            }

            var assignedTheatre =
                await theatreRepository.GetByManagerUserIdAsync(managerUserId.Value);

            if (assignedTheatre == null ||
                screen.TheatreId != assignedTheatre.TheatreId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to delete screens outside your assigned theatre.");
            }
        }

        return await repository.DeleteAsync(id);
    }
}
