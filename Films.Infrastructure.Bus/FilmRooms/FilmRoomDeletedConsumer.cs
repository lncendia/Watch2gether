using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using MassTransit;
using MassTransit.Mediator;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Infrastructure.Bus.FilmRooms;

/// <summary>
/// Обработчик интеграционного события FilmRoomDeletedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class FilmRoomDeletedConsumer(IMediator mediator) : IConsumer<FilmRoomDeletedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<FilmRoomDeletedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        return mediator.Send(new DeleteRoomCommand { RoomId = integrationEvent.Id }, context.CancellationToken);
    }
}