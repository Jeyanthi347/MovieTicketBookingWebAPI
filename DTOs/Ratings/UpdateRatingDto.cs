
namespace MovieTicketBookingWebAPI.DTOs.Ratings
{
    public class UpdateRatingDto
    {
        public decimal RatingValue { get; set; }

        public string? Review { get; set; }
    }
}
