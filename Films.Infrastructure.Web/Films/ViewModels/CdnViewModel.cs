namespace Films.Infrastructure.Web.Films.ViewModels;

public class CdnViewModel
{
    public required string Cdn { get; init; }
    public required string Quality { get; init; }
    public required IEnumerable<string> Voices { get; init; }
}