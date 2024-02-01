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
        if (request.PosterUrl != null) poster = await posterService.SaveAsync(request.PosterUrl);
        else if (request.PosterBase64 != null) poster = await posterService.SaveAsync(request.PosterBase64);
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