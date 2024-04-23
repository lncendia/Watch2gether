using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Playlists.InputModels;

public class PlaylistsSearchInputModel
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public Guid? FilmId { get; init; }
    public int Page { get; init; } = 1;
    [Range(1, 30)] public int CountPerPage { get; init; } = 15;
}