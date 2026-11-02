
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

    // Screen table
    public DbSet<Screen> Screens { get; set; }

    // Seat table
    public DbSet<Seat> Seats { get; set; }

    // Show table
    public DbSet<Show> Shows { get; set; }

    // ShowSeat table
    public DbSet<ShowSeat> ShowSeats { get; set; }

    // Booking table
    public DbSet<Booking> Bookings { get; set; }

    // BookingSeat table
    public DbSet<BookingSeat> BookingSeats { get; set; }

    // Rating table
    public DbSet<Rating> Ratings { get; set; }

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

        // Screen table configuration
        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(s => s.ScreenId);

            entity.Property(s => s.ScreenName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasOne<Theatre>()
                  .WithMany()
                  .HasForeignKey(s => s.TheatreId);
        });

        // Seat table configuration
        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(s => s.SeatId);

            entity.Property(s => s.RowName)
                  .IsRequired()
                  .HasMaxLength(5);

            entity.Property(s => s.SeatNumber)
                  .IsRequired();

            entity.Property(s => s.SeatType)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.HasOne<Screen>()
                  .WithMany()
                  .HasForeignKey(s => s.ScreenId);

            entity.HasIndex(s => new
            {
                s.ScreenId,
                s.RowName,
                s.SeatNumber
            }).IsUnique();

            entity.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_Seats_SeatNumber",
                    "[SeatNumber] > 0"));
        });

        // Show table configuration
        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(s => s.ShowId);

            entity.Property(s => s.StartTime)
                  .IsRequired();

            entity.Property(s => s.EndTime)
                  .IsRequired();

            entity.Property(s => s.TicketPrice)
                  .HasColumnType("decimal(10,2)")
                  .IsRequired();

            entity.HasOne<Movie>()
                  .WithMany()
                  .HasForeignKey(s => s.MovieId);

            entity.HasOne<Screen>()
                  .WithMany()
                  .HasForeignKey(s => s.ScreenId);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Shows_EndTime",
                    "[EndTime] > [StartTime]");

                t.HasCheckConstraint(
                    "CK_Shows_TicketPrice",
                    "[TicketPrice] >= 0");
            });
        });

        // ShowSeat table configuration
        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.HasKey(ss => ss.ShowSeatId);

            entity.Property(ss => ss.Status)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasOne<Show>()
                  .WithMany()
                  .HasForeignKey(ss => ss.ShowId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne<Seat>()
                  .WithMany()
                  .HasForeignKey(ss => ss.SeatId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(ss => new { ss.ShowId, ss.SeatId })
                  .IsUnique();
        });

        // Booking table configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.BookingId);

            entity.Property(b => b.BookingReference)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.HasIndex(b => b.BookingReference)
                  .IsUnique();

            entity.Property(b => b.CustomerName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(b => b.CustomerEmail)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(b => b.CustomerPhone)
                  .IsRequired()
                  .HasMaxLength(15);

            entity.Property(b => b.BookingDate)
                  .IsRequired();

            entity.Property(b => b.TotalAmount)
                  .HasColumnType("decimal(10,2)")
                  .IsRequired();

            entity.Property(b => b.Status)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasOne<Show>()
                  .WithMany()
                  .HasForeignKey(b => b.ShowId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Bookings_TotalAmount",
                    "[TotalAmount] >= 0");
            });
        });

        // BookingSeat table configuration
        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.HasKey(bs => bs.BookingSeatId);

            entity.Property(bs => bs.Price)
                  .HasColumnType("decimal(10,2)")
                  .IsRequired();

            entity.HasOne<Booking>()
                  .WithMany()
                  .HasForeignKey(bs => bs.BookingId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne<Seat>()
                  .WithMany()
                  .HasForeignKey(bs => bs.SeatId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_BookingSeats_Price",
                    "[Price] >= 0");
            });
        });

        // Rating table configuration
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(r => r.RatingId);

            entity.Property(r => r.RatingValue)
                  .HasPrecision(3, 1)
                  .IsRequired();

            entity.Property(r => r.Review)
                  .HasMaxLength(1000);

            // Rating must belong to a booking
            entity.HasOne(r => r.Booking)
                  .WithMany()
                  .HasForeignKey(r => r.BookingId)
                  .OnDelete(DeleteBehavior.NoAction);

            // Movie rating relationship
            entity.HasOne(r => r.Movie)
                  .WithMany()
                  .HasForeignKey(r => r.MovieId)
                  .OnDelete(DeleteBehavior.NoAction);

            // Theatre rating relationship
            entity.HasOne(r => r.Theatre)
                  .WithMany()
                  .HasForeignKey(r => r.TheatreId)
                  .OnDelete(DeleteBehavior.NoAction);

            // Prevent duplicate movie ratings for the same booking
            entity.HasIndex(r => new { r.BookingId, r.MovieId })
                  .IsUnique()
                  .HasFilter("[MovieId] IS NOT NULL");

            // Prevent duplicate theatre ratings for the same booking
            entity.HasIndex(r => new { r.BookingId, r.TheatreId })
                  .IsUnique()
                  .HasFilter("[TheatreId] IS NOT NULL");

            entity.ToTable(t =>
            {
                // Exactly one target must be selected
                t.HasCheckConstraint(
                    "CK_Ratings_OneTarget",
                    "([MovieId] IS NOT NULL AND [TheatreId] IS NULL) OR " +
                    "([MovieId] IS NULL AND [TheatreId] IS NOT NULL)");

                // Rating must be between 1 and 10
                t.HasCheckConstraint(
                    "CK_Ratings_RatingValue",
                    "[RatingValue] >= 1 AND [RatingValue] <= 10");
            });
        });
    }
}
