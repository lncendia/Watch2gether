using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Messages.YoutubeMessages;

namespace Room.Application.Services.CommandHandlers.YoutubeRooms;

/// <summary>
/// Обработчик команды на отправку сообщения
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class SendMessageCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<SendMessageCommand>
{
    public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetYoutubeRoomFromCache(request.RoomId, unitOfWork);

        var message = new YoutubeMessage(room, request.ViewerId, request.Message) { Id = Guid.NewGuid() };

        // Обновляем комнату в репозитории
        await unitOfWork.YoutubeMessageRepository.Value.AddAsync(message);
        
        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();
    }
}