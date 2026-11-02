
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingWebAPI.Models
{
    public class Theatre
    {
        public int TheatreId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public int? TheatreManagerUserId { get; set; }

        [ForeignKey(nameof(TheatreManagerUserId))]
        public User? TheatreManager { get; set; }
    }
}
