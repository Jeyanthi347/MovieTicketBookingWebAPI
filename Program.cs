
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;

using MovieTicketBookingWebAPI.Data;
using MovieTicketBookingWebAPI.Models;
using MovieTicketBookingWebAPI.Repositories;
using MovieTicketBookingWebAPI.Repositories.Interfaces;
using MovieTicketBookingWebAPI.Services;
using MovieTicketBookingWebAPI.Services.Interfaces;
using MovieTicketBookingWebAPI.Authorization.Handlers;
using MovieTicketBookingWebAPI.Authorization.Requirements;
using MovieTicketBookingWebAPI.Authorization.Policies;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Add OpenAPI
builder.Services.AddOpenApi();

// Movie registration
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// Theatre registration
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();

// Screen registration
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();

// Seat registration
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

// Show registration
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

// Booking registration
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Custom Authorization Handler registration
builder.Services.AddScoped<IAuthorizationHandler, BookingOwnerAuthorizationHandler>();

// Rating registration
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingService, RatingService>();

// User authentication registration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Password hashing registration
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is missing.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is missing.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT Audience is missing.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        ),

        ClockSkew = TimeSpan.Zero
    };
});

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.BookingOwner, policy =>
    {
        policy.RequireAuthenticatedUser();

        policy.AddRequirements(
            new BookingOwnerRequirement());
    });
});

var app = builder.Build();

// 1. HTTPS Redirection
app.UseHttpsRedirection();

// 2. Routing
//app.UseRouting();

// 3. Endpoint Metadata Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}

// 4. Authentication
// Validates JWT and populates HttpContext.User
app.UseAuthentication();

// 5. Authentication and Authorization Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();

    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}

// 6. Authorization
// Checks endpoint access permissions
app.UseAuthorization();


// ==========================================
// 12. MAP CONTROLLERS
// ==========================================

app.MapControllers();


// ==========================================
// 13. RUN APPLICATION
// ==========================================

app.Run();


// Configure HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//// Enable CORS
//app.UseCors("AngularPolicy");

//// Authentication must come before Authorization
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
