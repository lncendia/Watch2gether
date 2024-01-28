using Films.Domain.Films.ValueObjects;
using MediatR;

namespace Films.Application.Abstractions.Commands.FilmsManagement;

public class ChangeFilmCommand : IRequest
{
    public required Guid Id { get; init; }
    public string? Description { get; init; }
    public string? ShortDescription { get; init; }
    public Uri? PosterUri { get; init; }
    public Stream? PosterStream { get; init; }
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public int? CountSeasons { get; init; }
    public int? CountEpisodes { get; init; }
    public IReadOnlyCollection<Cdn>? CdnList { get; init; }
}