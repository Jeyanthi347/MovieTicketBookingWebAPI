namespace MovieTicketBookingWebAPI.DTOs;

public class TheatreResponseDto
{
    public int TheatreId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;
}