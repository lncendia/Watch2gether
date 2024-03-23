using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Rooms.InputModels;

public class RoomSearchInputModel
{
    public bool OnlyPublic { get; init; }
    public bool OnlyMy { get; init; }
    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;
    [Range(1, 15)] public int CountPerPage { get; init; } = 15;
}