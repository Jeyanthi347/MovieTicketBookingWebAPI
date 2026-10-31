
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingWebAPI.DTOs;

public class CreateMovieDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string MovieName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Range(1, 500)]
    public int DurationMinutes { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Language { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    [Range(typeof(decimal), "0", "10")]
    public decimal Rating { get; set; }
}
