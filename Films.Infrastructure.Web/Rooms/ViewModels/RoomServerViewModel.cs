namespace Films.Infrastructure.Web.Rooms.ViewModels;

public class RoomServerViewModel
{
    public required Guid Id { get; init; }
    public required string Url { get; init; }
    public string? Code { get; init; }
}