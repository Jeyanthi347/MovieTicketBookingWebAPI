
# Movie Ticket Booking System

## 1. Project Overview

The Movie Ticket Booking System is a backend application developed using ASP.NET Core Web API, .NET 10, Entity Framework Core 10 and SQL Server.

The application manages movies, theatres, screens, physical seats, movie shows, seat availability, customer bookings and booking cancellations.

## 2. Project Objectives

- Build REST APIs using ASP.NET Core Web API.
- Manage movie and theatre information.
- Schedule movies on theatre screens.
- Track seat availability for individual shows.
- Allow customers to book and cancel tickets.
- Use Entity Framework Core for database operations.
- Follow Controller-Service-Repository architecture.
- Prepare APIs for future Angular frontend integration.

## 3. Technology Stack

| Technology | Purpose |
|---|---|
| C# | Programming language |
| .NET 10 | Application framework |
| ASP.NET Core Web API | REST API development |
| Entity Framework Core 10 | Object-relational mapping |
| SQL Server | Database |
| SSMS | Database management |
| Postman | API testing |
| OpenAPI | API documentation |
| Git and GitHub | Version control |
| Angular | Future frontend |

## 4. Architecture

Controller → Service → Repository → ApplicationDbContext → SQL Server

### Controllers
Receive HTTP requests and return appropriate HTTP responses.

### Services
Handle business rules, workflows and DTO conversions.

### Repositories
Perform database operations using Entity Framework Core.

### Models
Represent database entities and relationships.

### DTOs
Define request and response structures.

### Data
Contains ApplicationDbContext and database configurations.

## 5. Functional Modules

1. Movies
2. Theatres
3. Screens
4. Seats
5. Shows
6. ShowSeats
7. Bookings
8. BookingSeats

## 6. Movie Rating Feature

Each movie has one common rating similar to IMDb.

- Rating range: 0 to 10.
- Database type: decimal(3,1).
- Example: 8.5.
- The administrator enters and updates the rating.
- Customers do not submit individual ratings.
- No separate MovieRating table is required.

## 7. Version 1 Scope

- Movie management.
- Theatre management.
- Screen management.
- Physical seat management.
- Show scheduling.
- Show-wise seat availability.
- Customer booking.
- Booking cancellation.

## 8. Version 1 Limitations

- No login or registration.
- No JWT authentication.
- No real payment gateway.
- No advanced concurrency protection.
- No Angular frontend in this phase.
- No email or SMS notifications.

## 9. Future Enhancements

- Authentication and authorization.
- Payment gateway integration.
- Angular frontend.
- Advanced seat concurrency handling.
- Email and SMS notifications.
- Production deployment.
