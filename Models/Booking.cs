//namespace MovieTicketBookingWebAPI.Models;

//public class Booking
//{
//    public int BookingId { get; set; }

//    public string BookingReference { get; set; } = string.Empty;

//    public int ShowId { get; set; }

//    public string CustomerName { get; set; } = string.Empty;

//    public string CustomerEmail { get; set; } = string.Empty;

//    public string CustomerPhone { get; set; } = string.Empty;

//    public DateTime BookingDate { get; set; }

//    public decimal TotalAmount { get; set; }

//    public string Status { get; set; } = "Confirmed";
//}

namespace MovieTicketBookingWebAPI.Models;

public class Booking
{
    public int BookingId { get; set; }

    public string BookingReference { get; set; } = string.Empty;

    public int ShowId { get; set; }

    // Customer account relationship
    public int? UserId { get; set; }

    public User? User { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Confirmed";
}