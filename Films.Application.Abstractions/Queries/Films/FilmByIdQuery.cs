using Films.Application.Abstractions.Queries.Films.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Films;

public class FilmByIdQuery : IRequest<FilmDto>
{
    public required Guid Id { get; init; }
    public Guid? UserId { get; init; }
}