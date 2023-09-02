using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.Films;

public class FilmsSearchParameters
{
    public string? Query { get; set; }
    public string? Genre { get; set; }
    public string? Person { get; set; }
    public string? Country { get; set; }
    public FilmType? Type { get; set; }
    public Guid? PlaylistId { get; set; }

    public int Page { get; set; } = 1;
}