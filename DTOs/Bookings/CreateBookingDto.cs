namespace MovieTicketBookingWebAPI.DTOs.Bookings;

public class CreateBookingDto
{
    public string CustomerPhone { get; set; } = string.Empty;

    public int ShowId { get; set; }

    public List<int> SeatIds { get; set; } = new List<int>();
}