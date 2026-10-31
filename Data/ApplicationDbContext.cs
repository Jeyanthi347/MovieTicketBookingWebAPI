
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Models;

namespace MovieTicketBookingWebAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Movie table
    public DbSet<Movie> Movies { get; set; }
}
