using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using Films.Domain.Abstractions.Interfaces;
using MediatR;

namespace Films.Application.Services.CommandHandlers.PlaylistManagement;

public class DeletePlaylistCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService)
    : IRequestHandler<DeletePlaylistCommand>
{
    public async Task Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await unitOfWork.PlaylistRepository.Value.GetAsync(request.Id) ??
                       throw new PlaylistNotFoundException();
        await posterService.DeleteAsync(playlist.PosterUrl);
        await unitOfWork.PlaylistRepository.Value.DeleteAsync(playlist.Id);
        await unitOfWork.SaveChangesAsync();
    }
}