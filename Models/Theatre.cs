using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingWebAPI.Models
{
    public class Theatre
    {
        public int TheatreId { get; set; }

        // Maps Name property to the SQL Server Name column
        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        // Theatre manager relationship
        public int? TheatreManagerUserId { get; set; }

        // Navigation property
        [ForeignKey(nameof(TheatreManagerUserId))]
        public User? TheatreManager { get; set; }
        public string TheatreName { get; internal set; }
    }
}