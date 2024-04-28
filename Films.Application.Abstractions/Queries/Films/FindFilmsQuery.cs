using Films.Application.Abstractions.DTOs.Common;
using Films.Application.Abstractions.DTOs.Films;
using MediatR;

namespace Films.Application.Abstractions.Queries.Films;

public class FindFilmsQuery : IRequest<ListDto<FilmShortDto>>
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public string? Country { get; init; }
    public string? Person { get; init; }
    public bool? Serial { get; init; }
    public int? MinYear { get; init; }
    public int? MaxYear { get; init; }
    public Guid? PlaylistId { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}