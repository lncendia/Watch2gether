using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Contracts.Films;

public class SearchParameters
{
    public string? Title { get; set; }
    public string? Person { get; set; }
    public string? Genre { get; set; }
    public string? Country { get; set; }
    public FilmType? Type { get; set; }
    public Guid? PlaylistId { get; set; }
}