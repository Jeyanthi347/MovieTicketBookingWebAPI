
using MovieTicketBookingWebAPI.DTOs.Auth;

namespace MovieTicketBookingWebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
