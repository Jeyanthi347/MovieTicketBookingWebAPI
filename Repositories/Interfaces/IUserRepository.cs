
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByIdAsync(int userId);

        Task<bool> EmailExistsAsync(string email);
    }
}
