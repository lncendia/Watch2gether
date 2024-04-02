using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Posters;
using Films.Application.Services.Extensions;
using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.FilmsManagement;

public class DeleteFilmCommandHandler(IUnitOfWork unitOfWork, IPosterStore posterStore, IMemoryCache memoryCache)
    : IRequestHandler<DeleteFilmCommand>
{
    public async Task Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await memoryCache.TryGetFilmFromCacheAsync(request.Id, unitOfWork);
        await posterStore.DeleteAsync(film.PosterUrl);
        await unitOfWork.FilmRepository.Value.DeleteAsync(request.Id);
        await unitOfWork.SaveChangesAsync();
        memoryCache.Remove(request.Id);
    }
}