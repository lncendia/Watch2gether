using Films.Application.Abstractions.DTOs.Playlists;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Abstractions.Queries.Playlists;
using Films.Application.Services.Mappers.Playlists;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Ordering;
using Films.Domain.Playlists;
using Films.Domain.Playlists.Ordering;
using Films.Domain.Playlists.Ordering.Visitor;
using Films.Domain.Playlists.Specifications;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Playlists;

public class PlaylistByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<PlaylistByIdQuery, PlaylistDto>
{
    public async Task<PlaylistDto> Handle(PlaylistByIdQuery request, CancellationToken cancellationToken)
    {
        var playlist = await unitOfWork.PlaylistRepository.Value.GetAsync(request.Id);

        if (playlist == null) throw new PlaylistNotFoundException();

        return Mapper.Map(playlist);
    }
}