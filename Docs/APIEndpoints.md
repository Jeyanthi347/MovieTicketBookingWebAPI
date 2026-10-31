
# API Endpoints

Base URL: https://localhost:{port}/api

All endpoints are publicly accessible in Version 1.

## 1. Movies

| Method | Endpoint | Description |
|---|---|---|
| POST | /Movies | Create a movie |
| GET | /Movies | Get all movies |
| GET | /Movies/{id} | Get movie by ID |
| PUT | /Movies/{id} | Update a movie |
| DELETE | /Movies/{id} | Delete a movie |
| GET | /Movies/search?title=leo | Search movies by title |

### Create Movie Request

POST /Movies

{
  "movieName": "Leo",
  "genre": "Action",
  "durationMinutes": 164,
  "description": "Action thriller",
  "language": "Tamil",
  "releaseDate": "2023-10-19",
  "rating": 7.2
}

### Movie Rating

- Rating range: 0 to 10.
- Rating precision: One decimal place.
- Rating is maintained by the administrator.
- Individual user ratings are not supported.

## 2. Theatres

| Method | Endpoint | Description |
|---|---|---|
| POST | /Theatres | Create theatre |
| GET | /Theatres | Get all theatres |
| GET | /Theatres/{id} | Get theatre by ID |
| PUT | /Theatres/{id} | Update theatre |
| DELETE | /Theatres/{id} | Delete theatre |

## 3. Screens

| Method | Endpoint | Description |
|---|---|---|
| POST | /Screens | Create screen |
| GET | /Screens | Get all screens |
| GET | /Screens/{id} | Get screen by ID |
| GET | /Screens/theatre/{theatreId} | Get screens in theatre |
| PUT | /Screens/{id} | Update screen |
| DELETE | /Screens/{id} | Delete screen |

## 4. Seats

| Method | Endpoint | Description |
|---|---|---|
| POST | /Seats | Create seat |
| POST | /Seats/bulk | Create multiple seats |
| GET | /Seats/screen/{screenId} | Get seats in a screen |
| GET | /Seats/{id} | Get seat by ID |
| PUT | /Seats/{id} | Update seat |
| DELETE | /Seats/{id} | Delete seat |

## 5. Shows

| Method | Endpoint | Description |
|---|---|---|
| POST | /Shows | Schedule a show |
| GET | /Shows | Get all shows |
| GET | /Shows/{id} | Get show by ID |
| GET | /Shows/movie/{movieId} | Get shows for a movie |
| GET | /Shows/theatre/{theatreId} | Get shows in a theatre |
| GET | /Shows/search?date=YYYY-MM-DD | Search shows by date |
| PUT | /Shows/{id} | Update show |
| DELETE | /Shows/{id} | Delete show |

## 6. ShowSeats

Base route: /Shows/{showId}/seats

| Method | Endpoint | Description |
|---|---|---|
| GET | /Shows/{showId}/seats | Get seat layout and status |
| GET | /Shows/{showId}/seats/available | Get available seats |
| GET | /Shows/{showId}/seats/booked | Get booked seats |

ShowSeats records are generated automatically when a show is created.

## 7. Bookings

| Method | Endpoint | Description |
|---|---|---|
| POST | /Bookings | Create booking |
| GET | /Bookings | Get all bookings |
| GET | /Bookings/{id} | Get booking details |
| GET | /Bookings/show/{showId} | Get bookings for a show |
| PUT | /Bookings/{id}/cancel | Cancel a booking |

## 8. HTTP Status Codes

| Status | Meaning |
|---|---|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 404 | Not Found |
| 409 | Conflict |
| 500 | Internal Server Error |

Note: This document describes the planned Version 1 API specification. Endpoints become available as their respective modules are implemented.
