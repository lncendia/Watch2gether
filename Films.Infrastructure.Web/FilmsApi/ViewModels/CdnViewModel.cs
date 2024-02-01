using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.FilmsApi.ViewModels;

public class CdnViewModel
{
    public required CdnType Cdn { get; init; }
    public required string Quality { get; init; }
    public required IEnumerable<string> Voices { get; init; }
}