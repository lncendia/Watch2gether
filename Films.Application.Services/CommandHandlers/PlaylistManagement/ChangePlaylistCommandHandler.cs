using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.PlaylistManagement;

public class ChangePlaylistCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService)
    : IRequestHandler<ChangePlaylistCommand>
{
    public async Task Handle(ChangePlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await unitOfWork.PlaylistRepository.Value.GetAsync(request.Id);
        if (playlist == null) throw new PlaylistNotFoundException();
        if (!string.IsNullOrEmpty(request.Name)) playlist.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Description)) playlist.Description = request.Description;
        Uri? poster = null;
        if (request.PosterUrl != null) poster = await posterService.SaveAsync(request.PosterUrl);
        else if (request.PosterBase64 != null)
            poster = await posterService.SaveAsync(request.PosterBase64);

        if (poster != null)
        {
            await posterService.DeleteAsync(playlist.PosterUrl);
            playlist.PosterUrl = poster;
        }


        if (request.Films != null) playlist.UpdateFilms(request.Films);

        await unitOfWork.PlaylistRepository.Value.UpdateAsync(playlist);
        await unitOfWork.SaveChangesAsync();
    }
}