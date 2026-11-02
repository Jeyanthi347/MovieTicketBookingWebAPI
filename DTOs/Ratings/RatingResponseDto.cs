
namespace MovieTicketBookingWebAPI.DTOs.Ratings
{
    public class RatingResponseDto
    {
        public int RatingId { get; set; }

        public int BookingId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string? MovieName { get; set; }

        public string? TheatreName { get; set; }

        public decimal RatingValue { get; set; }

        public string? Review { get; set; }
    }
}
