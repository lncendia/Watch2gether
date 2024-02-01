namespace Films.Infrastructure.Web.Profile.ViewModels;

public class FilmViewModel
{
    public required string Name { get; init; }
    public required int Year { get; init; }
    public required Guid Id { get; init; }
    public required Uri PosterUrl { get; init; }
}