using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingWebAPI.DTOs;

public class CreateSeatDto
{
    [Required]
    public int ScreenId { get; set; }

    [Required]
    [MaxLength(5)]
    public string RowName { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int SeatNumber { get; set; }

    [Required]
    [MaxLength(30)]
    public string SeatType { get; set; } = string.Empty;
}