using Films.Application.Abstractions.Commands.Profile;
using Films.Application.Abstractions.Exceptions;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Films.Application.Services.CommandHandlers.Profile;

public class AddToHistoryCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IRequestHandler<AddToHistoryCommand>
{
    public async Task Handle(AddToHistoryCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя по его идентификатору 
        var user = await unitOfWork.UserRepository.Value.GetAsync(request.UserId);
        if (user == null) throw new UserNotFoundException();

        var film = await memoryCache.TryGetFilmFromCacheAsync(request.FilmId, unitOfWork);
        
        // Добавляем фильм в историю пользователя 
        user.AddFilmToHistory(film);

        // Обновляем данные пользователя в репозитории 
        await unitOfWork.UserRepository.Value.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}