using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingWebAPI.DTOs;

public class CreateShowDto
{
    [Required]
    public int MovieId { get; set; }

    [Required]
    public int ScreenId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [Range(0, 99999999.99)]
    public decimal TicketPrice { get; set; }
}