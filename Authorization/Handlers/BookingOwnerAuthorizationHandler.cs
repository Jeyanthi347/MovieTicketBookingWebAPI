using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Authorization.Requirements;

namespace MovieTicketBookingWebAPI.Authorization.Handlers;

public class BookingOwnerAuthorizationHandler
    : AuthorizationHandler<BookingOwnerRequirement, HttpContext>
{
    private readonly IBookingRepository repository;

    public BookingOwnerAuthorizationHandler(
        IBookingRepository repository)
    {
        this.repository = repository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BookingOwnerRequirement requirement,
        HttpContext httpContext)
    {
        // Allow Admin users
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        // Get logged-in user's ID from JWT token
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return;

        // Get booking ID from URL
        var bookingIdValue =
            httpContext.Request.RouteValues["id"]?.ToString();

        if (!int.TryParse(bookingIdValue, out int bookingId))
            return;

        // Get booking from database
        var booking = await repository.GetByIdAsync(bookingId);

        if (booking == null)
            return;

        // Check booking ownership
        if (booking.UserId == userId)
        {
            context.Succeed(requirement);
        }
    }
}