using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Infrastructure.Bus.FilmRooms;

/// <summary>
/// Обработчик интеграционного события FilmRoomCreatedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class FilmRoomViewerKickedConsumer(IMediator mediator) : IConsumer<FilmRoomViewerKickedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<FilmRoomViewerKickedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        return mediator.Send(new BlockViewerCommand
        {
            UserId = integrationEvent.ViewerId,
            RoomId = integrationEvent.RoomId
        }, context.CancellationToken);
    }
}