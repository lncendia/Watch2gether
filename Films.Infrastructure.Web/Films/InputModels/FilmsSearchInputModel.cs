using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Films.InputModels;

public class FilmsSearchInputModel
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public string? Person { get; init; }
    public string? Country { get; init; }
    public bool? Serial { get; init; }
    public Guid? PlaylistId { get; init; }
    [Range(1, int.MaxValue)] public int Page { get; init; } = 1;
    [Range(1, 15)] public int CountPerPage { get; init; } = 15;
}