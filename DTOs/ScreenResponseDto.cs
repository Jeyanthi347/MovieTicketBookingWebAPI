namespace MovieTicketBookingWebAPI.DTOs;

public class ScreenResponseDto
{
    public int ScreenId { get; set; }

    public int TheatreId { get; set; }

    public string ScreenName { get; set; } = string.Empty;
}