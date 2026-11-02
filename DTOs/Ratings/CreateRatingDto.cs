
namespace MovieTicketBookingWebAPI.DTOs.Ratings
{
    public class CreateRatingDto
    {
        public int BookingId { get; set; }

        public int? MovieId { get; set; }

        public int? TheatreId { get; set; }

        public decimal RatingValue { get; set; }

        public string? Review { get; set; }
    }
}
