using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.Contracts.Films;

public class FilmsSearchParameters
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public string? Person { get; init; }
    public string? Country { get; init; }
    public FilmType? Type { get; init; }
    public Guid? PlaylistId { get; init; }

    public int Page { get; init; } = 1;
}