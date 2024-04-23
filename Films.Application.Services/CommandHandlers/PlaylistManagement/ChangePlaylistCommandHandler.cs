using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Abstractions.Posters;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.PlaylistManagement;

public class ChangePlaylistCommandHandler(IUnitOfWork unitOfWork, IPosterStore posterStore)
    : IRequestHandler<ChangePlaylistCommand>
{
    public async Task Handle(ChangePlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await unitOfWork.PlaylistRepository.Value.GetAsync(request.Id);
        if (playlist == null) throw new PlaylistNotFoundException();
        if (!string.IsNullOrEmpty(request.Name)) playlist.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Description)) playlist.Description = request.Description;
        Uri? poster = null;
        if (request.PosterUrl != null) poster = await posterStore.SaveAsync(request.PosterUrl);
        else if (request.PosterBase64 != null)
            poster = await posterStore.SaveAsync(request.PosterBase64);

        if (poster != null)
        {
            await posterStore.DeleteAsync(playlist.PosterUrl);
            playlist.PosterUrl = poster;
        }


        if (request.Films != null) playlist.UpdateFilms(request.Films);

        await unitOfWork.PlaylistRepository.Value.UpdateAsync(playlist);
        await unitOfWork.SaveChangesAsync();
    }
}