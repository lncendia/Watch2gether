using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Domain.Abstractions.Interfaces;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на удаление комнаты ютуб
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class DeleteRoomCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<DeleteRoomCommand>
{
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        // Удаляем комнату из репозитория
        await unitOfWork.YoutubeRoomRepository.Value.DeleteAsync(request.Id);
        
        // Удаляем комнату из кеша
        cache.Remove(request.Id);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}