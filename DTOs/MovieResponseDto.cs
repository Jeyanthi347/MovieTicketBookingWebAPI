
namespace MovieTicketBookingWebAPI.DTOs;

public class MovieResponseDto
{
    public int MovieId { get; set; }

    public string MovieName { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    public decimal Rating { get; set; }

    public bool IsActive { get; set; }
}
