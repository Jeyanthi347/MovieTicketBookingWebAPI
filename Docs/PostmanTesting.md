
# Postman API Testing

## Base URL

https://localhost:{port}/api

## 1. Movie API Testing

### Create Movie

Method: POST

URL: /Movies

Body → raw → JSON

{
  "movieName": "Leo",
  "genre": "Action",
  "durationMinutes": 164,
  "description": "Action thriller",
  "language": "Tamil",
  "releaseDate": "2023-10-19",
  "rating": 7.2
}

Expected status: 201 Created.

### Get All Movies

Method: GET

URL: /Movies

Expected status: 200 OK.

### Get Movie By ID

Method: GET

URL: /Movies/1

Expected status: 200 OK.

For an invalid ID: 404 Not Found.

### Update Movie

Method: PUT

URL: /Movies/1

Body → raw → JSON

{
  "movieName": "Leo",
  "genre": "Action",
  "durationMinutes": 164,
  "description": "Updated action thriller",
  "language": "Tamil",
  "releaseDate": "2023-10-19",
  "rating": 8.1,
  "isActive": true
}

Expected status: 200 OK.

### Delete Movie

Method: DELETE

URL: /Movies/1

Expected status: 204 No Content.

If dependent show or booking history exists, deletion should be rejected.

## 2. Movie Rating Validation

| Rating | Expected result |
|---|---|
| 8.5 | Valid |
| 7.2 | Valid |
| 0 | Valid |
| 10 | Valid |
| 10.5 | Invalid |
| -1 | Invalid |

## 3. Theatre Testing

Test theatre creation, retrieval, updating and deletion.

## 4. Screen Testing

Test screen creation and verify that TheatreId references an existing theatre.

## 5. Seat Testing

Test individual and bulk seat creation.

Verify duplicate seat coordinates are rejected.

## 6. Show Testing

Create a show using an existing MovieId and ScreenId.

Verify that ShowSeats records are automatically generated.

## 7. ShowSeat Testing

Retrieve the show seat layout.

Verify that seats have Available or Booked status.

## 8. Booking Testing

Create a booking using an existing ShowId and available SeatIds.

Verify:
- Booking record is created.
- BookingSeats records are created.
- TotalAmount is calculated by the backend.
- ShowSeats status changes to Booked.

## 9. Cancellation Testing

Cancel a confirmed booking.

Verify:
- Booking status changes to Cancelled.
- Booking history remains in the database.
- Corresponding ShowSeats status changes to Available.

## 10. Important Test Cases

- Invalid movie ID.
- Invalid movie rating.
- Missing required movie fields.
- Duplicate physical seats.
- Invalid screen or theatre ID.
- Overlapping show schedules.
- Booking an already-booked seat.
- Booking with duplicate SeatIds.
- Cancelling an already-cancelled booking.
- Deleting records with dependent history.
