using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Playlists.Entities;

namespace Films.Application.Services.Commands.PlaylistManagement;

public class CreatePlaylistCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService)
    : IRequestHandler<CreatePlaylistCommand, Guid>
{
    public async Task<Guid> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
    {
        Uri? poster;
        if (request.PosterUri != null) poster = await posterService.SaveAsync(request.PosterUri);
        else if (request.PosterStream != null) poster = await posterService.SaveAsync(request.PosterStream);
        else throw new PosterMissingException();
        var playlist = new Playlist
        {
            Name = request.Name,
            Description = request.Description,
            PosterUrl = poster
        };
        await unitOfWork.PlaylistRepository.Value.AddAsync(playlist);
        await unitOfWork.SaveChangesAsync();
        return playlist.Id;
    }
}