using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Room.Application.Abstractions.DTOs.Messages;
using Room.Application.Abstractions.Exceptions;
using Room.Application.Abstractions.Queries.FilmRooms;
using Room.Application.Services.Common;
using Room.Application.Services.Mappers;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Messages.FilmMessages;
using Room.Domain.Messages.FilmMessages.Ordering;
using Room.Domain.Messages.FilmMessages.Ordering.Visitor;
using Room.Domain.Messages.FilmMessages.Specifications;
using Room.Domain.Messages.FilmMessages.Specifications.Visitor;
using Room.Domain.Ordering;
using Room.Domain.Specifications;
using Room.Domain.Specifications.Abstractions;

namespace Room.Application.Services.QueryHandlers.FilmRooms;

/// <summary>
/// Обработчик запроса для получения сообщений комнаты с фильмом.
/// </summary>
/// <param name="unitOfWork">Единица работы</param>
/// <param name="cache">Сервис кеша в памяти</param>
public class FilmRoomMessagesQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache)
    : IRequestHandler<FilmRoomMessagesQuery, (IReadOnlyCollection<MessageDto> messages, int count)>
{
    /// <summary>
    /// Обработчик запроса на получение сообщений комнаты фильма.
    /// </summary>
    public async Task<(IReadOnlyCollection<MessageDto> messages, int count)> Handle(FilmRoomMessagesQuery request,
        CancellationToken cancellationToken)
    {
        // Получение комнаты из кэша по идентификатору комнаты из запроса
        var room = await cache.TryGetFilmRoomFromCache(request.RoomId, unitOfWork);

        // Проверка, что все зрители комнаты не содержат идентификатор запросившего зрителя
        if (room.Viewers.All(v => v.Id != request.ViewerId)) throw new ViewerNoAccessException();

        // Создание спецификации для сообщений комнаты
        ISpecification<FilmMessage, IFilmMessageSpecificationVisitor> specification =
            new RoomMessagesSpecification(room.Id);

        // Создание объекта для сортировки сообщений по дате
        var order = new DescendingOrder<FilmMessage, IFilmMessageSortingVisitor>(new MessagesOrderByDate());

        // Проверка наличия идентификатора начального сообщения в запросе
        if (request.FromMessageId.HasValue)
        {
            // Получение сообщения по идентификатору из репозитория
            var message = await unitOfWork.FilmMessageRepository.Value.GetAsync(request.FromMessageId.Value);
            if (message == null) throw new MessageNotFoundException();

            // Добавление спецификации по дате создания сообщения к текущей спецификации
            specification = new AndSpecification<FilmMessage, IFilmMessageSpecificationVisitor>(specification,
                new MessagesFromDateSpecification(message.CreatedAt));
        }

        // Поиск сообщений с использованием спецификации и сортировки
        var messages = await unitOfWork.FilmMessageRepository.Value.FindAsync(specification, order, 0, request.Count);

        // Получение общего количества сообщений по спецификации
        var count = await unitOfWork.FilmMessageRepository.Value.CountAsync(specification);

        // Возврат массива отображенных сообщений и общего количества
        return (messages.Select(MessageMapper.Map).ToArray(), count);
    }
}