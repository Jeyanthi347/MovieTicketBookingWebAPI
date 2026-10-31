
# Database Design

## Database Information

- Database Name: MovieTicketBookingDB
- Database: SQL Server
- ORM: Entity Framework Core 10
- Total Tables: 8

## 1. Movies

Stores movie catalogue information.

| Column | Description |
|---|---|
| MovieId | Primary key; identity |
| MovieName | Movie title |
| Genre | Movie category |
| DurationMinutes | Movie duration |
| Description | Movie description |
| Language | Movie language |
| ReleaseDate | Movie release date |
| Rating | Common movie rating from 0 to 10 |
| IsActive | Indicates whether the movie is active |

Rating is stored as decimal(3,1).

Example: 8.5.

## 2. Theatres

Stores cinema theatre information.

| Column | Description |
|---|---|
| TheatreId | Primary key; identity |
| Name | Theatre name |
| Location | Theatre location |

## 3. Screens

Stores screens inside theatres.

| Column | Description |
|---|---|
| ScreenId | Primary key; identity |
| TheatreId | Foreign key referencing Theatres |
| ScreenName | Screen name |

## 4. Seats

Stores physical seats inside each screen.

| Column | Description |
|---|---|
| SeatId | Primary key; identity |
| ScreenId | Foreign key referencing Screens |
| RowName | Seat row |
| SeatNumber | Seat number |
| SeatType | Regular or Premium |

Unique constraint: ScreenId, RowName, SeatNumber.

## 5. Shows

Stores scheduled movie screenings.

| Column | Description |
|---|---|
| ShowId | Primary key; identity |
| MovieId | Foreign key referencing Movies |
| ScreenId | Foreign key referencing Screens |
| StartTime | Show start date and time |
| EndTime | Show end date and time |
| TicketPrice | Ticket price |

Rules:
- EndTime must be later than StartTime.
- TicketPrice must not be negative.
- A screen must not have overlapping shows.

## 6. ShowSeats

Tracks seat availability for each show.

| Column | Description |
|---|---|
| ShowSeatId | Primary key; identity |
| ShowId | Foreign key referencing Shows |
| SeatId | Foreign key referencing Seats |
| Status | Available or Booked |

Unique constraint: ShowId, SeatId.

## 7. Bookings

Stores customer and booking-level details.

| Column | Description |
|---|---|
| BookingId | Primary key; identity |
| BookingReference | Unique booking reference |
| ShowId | Foreign key referencing Shows |
| CustomerName | Customer name |
| CustomerEmail | Customer email |
| CustomerPhone | Customer phone |
| BookingDate | Booking date and time |
| TotalAmount | Total booking amount |
| Status | Confirmed or Cancelled |

## 8. BookingSeats

Stores the seats included in each booking.

| Column | Description |
|---|---|
| BookingSeatId | Primary key; identity |
| BookingId | Foreign key referencing Bookings |
| SeatId | Foreign key referencing Seats |
| Price | Price captured for the seat |

## Entity Relationships

- Theatre → Screens: One-to-many.
- Screen → Seats: One-to-many.
- Movie → Shows: One-to-many.
- Screen → Shows: One-to-many.
- Show → ShowSeats: One-to-many.
- Seat → ShowSeats: One-to-many.
- Show → Bookings: One-to-many.
- Booking → BookingSeats: One-to-many.
- Seat → BookingSeats: One-to-many.

## Important Database Rules

1. Use identity integer primary keys.
2. Use foreign keys for entity relationships.
3. Prevent duplicate seat coordinates in a screen.
4. Prevent duplicate ShowId and SeatId combinations in ShowSeats.
5. BookingReference must be unique.
6. Do not make SeatId globally unique in BookingSeats.
7. Use restrictive delete behavior for records with dependent history.
8. Preserve booking history when cancelling a booking.
