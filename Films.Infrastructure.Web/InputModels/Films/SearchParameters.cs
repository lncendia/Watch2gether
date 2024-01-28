using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.Contracts.Films;

public class SearchParameters
{
    public string? Title { get; init; }
    public string? Person { get; init; }
    public string? Genre { get; init; }
    public string? Country { get; init; }
    public FilmType? Type { get; init; }
    public Guid? PlaylistId { get; init; }
}