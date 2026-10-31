using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingWebAPI.DTOs;

public class UpdateTheatreDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;
}