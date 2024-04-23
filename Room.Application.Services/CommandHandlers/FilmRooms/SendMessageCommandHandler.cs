using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.DTOs.Messages;
using Room.Application.Services.Common;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Messages.FilmMessages;

namespace Room.Application.Services.CommandHandlers.FilmRooms;

/// <summary>
/// Обработчик команды на отправку сообщения
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class SendMessageCommandHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : IRequestHandler<SendMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Получаем комнату
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        var message = new FilmMessage(room, request.ViewerId, request.Message) { Id = Guid.NewGuid() };

        // Обновляем комнату в репозитории
        await unitOfWork.FilmMessageRepository.Value.AddAsync(message);

        // Сохраняем изменения
        await unitOfWork.SaveChangesAsync();

        // Возвращаем сообщение
        return Mappers.MessageMapper.Map(message);
    }
}