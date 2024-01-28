using Films.Application.Abstractions.Queries.Films.DTOs;
using Films.Domain.Films.Enums;
using MediatR;

namespace Films.Application.Abstractions.Queries.Films;

public class FindFilmsQuery : IRequest<IReadOnlyCollection<FilmShortDto>>
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public string? Country { get; init; }
    public string? Person { get; init; }
    public FilmType? Type { get; init; }
    public Guid? Playlist { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}