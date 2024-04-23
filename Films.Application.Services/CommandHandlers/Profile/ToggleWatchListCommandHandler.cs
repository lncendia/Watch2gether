using Films.Application.Abstractions.Commands.Profile;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.Profile;

public class ToggleWatchListCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    : IRequestHandler<ToggleWatchListCommand>
{
    public async Task Handle(ToggleWatchListCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        // Проверяем, находится ли фильм в списке просмотра пользователя 
        if (user.Watchlist.Any(x => x.FilmId == request.FilmId))
        {
            user.RemoveFilmFromWatchlist(request.FilmId); // Если да, удаляем фильм из списка просмотра 
        }
        else
        {
            var film = await memoryCache.TryGetFilmFromCacheAsync(request.FilmId, unitOfWork);
            user.AddFilmToWatchlist(film); // Если нет, добавляем фильм в список просмотра 
        }

        // Обновляем данные пользователя в репозитории 
        await unitOfWork.UserRepository.Value.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}