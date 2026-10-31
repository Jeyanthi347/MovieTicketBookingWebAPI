namespace MovieTicketBookingWebAPI.DTOs;

public class CreateBookingDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int ShowId { get; set; }
    public List<int> SeatIds { get; set; } = new List<int>();
}