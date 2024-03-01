using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Posters;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.PlaylistManagement;

public class DeletePlaylistCommandHandler(IUnitOfWork unitOfWork, IPosterStore posterStore)
    : IRequestHandler<DeletePlaylistCommand>
{
    public async Task Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await unitOfWork.PlaylistRepository.Value.GetAsync(request.Id) ??
                       throw new PlaylistNotFoundException();
        await posterStore.DeleteAsync(playlist.PosterUrl);
        await unitOfWork.PlaylistRepository.Value.DeleteAsync(playlist.Id);
        await unitOfWork.SaveChangesAsync();
    }
}