
namespace MovieTicketBookingWebAPI.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        public int BookingId { get; set; }

        public int? MovieId { get; set; }

        public int? TheatreId { get; set; }

        public decimal RatingValue { get; set; }

        public string? Review { get; set; }

        // Navigation properties
        public Booking Booking { get; set; } = null!;

        public Movie? Movie { get; set; }

        public Theatre? Theatre { get; set; }
    }
}
