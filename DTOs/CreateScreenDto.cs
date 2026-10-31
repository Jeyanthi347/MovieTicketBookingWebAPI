using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingWebAPI.DTOs;

public class CreateScreenDto
{
    [Required]
    public int TheatreId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ScreenName { get; set; } = string.Empty;
}