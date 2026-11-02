
using Microsoft.AspNetCore.Identity;
using MovieTicketBookingWebAPI.Constants;
using MovieTicketBookingWebAPI.DTOs.Auth;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services.Interfaces;

namespace MovieTicketBookingWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IJwtService jwtService;

        public UserService(
            IUserRepository repository,
            IPasswordHasher<User> passwordHasher,
            IJwtService jwtService)
        {
            this.repository = repository;
            this.passwordHasher = passwordHasher;
            this.jwtService = jwtService;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            string email = dto.Email.Trim().ToLowerInvariant();

            bool emailExists = await repository.EmailExistsAsync(email);

            if (emailExists)
            {
                return null;
            }

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = email,
                Role = UserRoles.Customer
            };

            user.PasswordHash = passwordHasher.HashPassword(
                user,
                dto.Password
            );

            await repository.CreateAsync(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = jwtService.GenerateToken(user)
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            string email = dto.Email.Trim().ToLowerInvariant();

            var user = await repository.GetByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = jwtService.GenerateToken(user)
            };
        }
    }
}
