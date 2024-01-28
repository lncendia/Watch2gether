using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;

namespace Films.Application.Services.Commands.PlaylistManagement;

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