using Microsoft.EntityFrameworkCore;
using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Repositories;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services;
using MovieTicketBookingWebAPI.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Configure SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Add OpenAPI
builder.Services.AddOpenApi();
// Repository registration
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// Service registration
builder.Services.AddScoped<IMovieService, MovieService>();


// Register TheatreRepository and TheatreService
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();


// Register ScreenRepository and ScreenService
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();

builder.Services.AddScoped<IScreenService, ScreenService>();


// Register SeatRepository and SeatService
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

//Register ShowRepository and ShowService

builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

//Register BookingRepository and BookingService

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();