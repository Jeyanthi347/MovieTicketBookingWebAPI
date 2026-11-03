namespace MovieTicketBookingWebAPI.DTOs.Common;

public class PaginationResponseDto<T>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalRecords { get; set; }

    public int TotalPages { get; set; }

    public List<T> Data { get; set; } = new List<T>();
}