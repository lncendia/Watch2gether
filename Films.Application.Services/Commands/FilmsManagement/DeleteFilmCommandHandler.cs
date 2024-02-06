using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Application.Abstractions.Common.Interfaces;
using Films.Application.Services.Common;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;

namespace Films.Application.Services.Commands.FilmsManagement;

public class DeleteFilmCommandHandler(IUnitOfWork unitOfWork, IPosterService posterService, IMemoryCache memoryCache)
    : IRequestHandler<DeleteFilmCommand>
{
    public async Task Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
    {
        var film = await memoryCache.TryGetFilmFromCache(request.Id, unitOfWork);
        await posterService.DeleteAsync(film.PosterUrl);
        await unitOfWork.FilmRepository.Value.DeleteAsync(request.Id);
        await unitOfWork.SaveChangesAsync();
        memoryCache.Remove(request.Id);
    }
}