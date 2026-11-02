
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
