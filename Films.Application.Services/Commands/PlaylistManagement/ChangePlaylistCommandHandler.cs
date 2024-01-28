using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;

namespace Films.Application.Services.Commands.PlaylistManagement;

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
        if (request.PosterUri != null) poster = await posterService.SaveAsync(request.PosterUri);
        else if (request.PosterStream != null)
            poster = await posterService.SaveAsync(request.PosterStream);

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