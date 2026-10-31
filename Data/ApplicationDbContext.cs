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

    // Theatre table
    public DbSet<Theatre> Theatres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Movie table configuration
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.MovieId);

            entity.Property(m => m.MovieName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(m => m.Genre)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(m => m.Description)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(m => m.Language)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(m => m.Rating)
                  .HasPrecision(3, 1);
        });

        // Theatre table configuration
        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.HasKey(t => t.TheatreId);

            entity.Property(t => t.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(t => t.Location)
                  .IsRequired()
                  .HasMaxLength(200);
        });
    }
}