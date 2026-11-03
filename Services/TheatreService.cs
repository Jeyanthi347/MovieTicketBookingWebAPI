
using MovieTicketBookingWebAPI.DTOs.Common;
using MovieTicketBookingWebAPI.DTOs.Theatres;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services;

public class TheatreService : ITheatreService
{
    private readonly ITheatreRepository repository;

    public TheatreService(ITheatreRepository repository)
    {
        this.repository = repository;
    }

    // Get paginated theatres
    public async Task<PaginationResponseDto<TheatreResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize)
    {
        var result = await repository.GetAllAsync(pageNumber, pageSize);

        var theatres = result.Theatres.Select(t => new TheatreResponseDto
        {
            TheatreId = t.TheatreId,
            Name = t.Name,
            Location = t.Location
        }).ToList();

        return new PaginationResponseDto<TheatreResponseDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = result.TotalRecords,
            TotalPages = (int)Math.Ceiling(
                result.TotalRecords / (double)pageSize),
            Data = theatres
        };
    }

    // Get theatre by ID
    public async Task<TheatreResponseDto?> GetByIdAsync(int id)
    {
        var theatre = await repository.GetByIdAsync(id);

        if (theatre == null)
        {
            return null;
        }

        return new TheatreResponseDto
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location
        };
    }

    // Create theatre
    public async Task<TheatreResponseDto> CreateAsync(CreateTheatreDto dto)
    {
        var theatre = new Theatre
        {
            Name = dto.Name,
            Location = dto.Location
        };

        var createdTheatre = await repository.AddAsync(theatre);

        return new TheatreResponseDto
        {
            TheatreId = createdTheatre.TheatreId,
            Name = createdTheatre.Name,
            Location = createdTheatre.Location
        };
    }

    // Update theatre
    public async Task<TheatreResponseDto?> UpdateAsync(
        int id,
        UpdateTheatreDto dto)
    {
        var theatre = await repository.GetByIdAsync(id);

        if (theatre == null)
        {
            return null;
        }

        theatre.Name = dto.Name;
        theatre.Location = dto.Location;

        var updatedTheatre = await repository.UpdateAsync(theatre);

        return new TheatreResponseDto
        {
            TheatreId = updatedTheatre.TheatreId,
            Name = updatedTheatre.Name,
            Location = updatedTheatre.Location
        };
    }

    // Delete theatre
    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }
}
