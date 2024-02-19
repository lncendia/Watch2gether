using Films.Domain.Films.ValueObjects;
using MediatR;

namespace Films.Application.Abstractions.Commands.FilmsManagement;

public class AddFilmCommand : IRequest<Guid>
{
    public required string Description { get; init; }
    public string? ShortDescription { get; init; }
    public required bool IsSerial { get; init; }
    public Uri? PosterUrl { get; init; }
    public string? PosterBase64 { get; init; }
    public required string Title { get; init; }
    public required int Year { get; init; }
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public int? CountSeasons { get; init; }
    public int? CountEpisodes { get; init; }
    public required IReadOnlyCollection<Cdn> CdnList { get; init; }
    public required IReadOnlyCollection<string> Countries { get; init; }
    public required IReadOnlyCollection<Actor> Actors { get; init; }
    public required IReadOnlyCollection<string> Directors { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
    public required IReadOnlyCollection<string> Screenwriters { get; init; }
}