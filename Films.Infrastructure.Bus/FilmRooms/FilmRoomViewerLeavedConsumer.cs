using Films.Application.Abstractions.Commands.FilmRooms;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;

namespace Films.Infrastructure.Bus.FilmRooms;

/// <summary>
/// Обработчик интеграционного события FilmRoomViewerLeavedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class FilmRoomViewerLeavedConsumer(IMediator mediator) : IConsumer<FilmRoomViewerLeavedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<FilmRoomViewerLeavedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        return mediator.Send(new DisconnectRoomCommand
        {
            UserId = integrationEvent.ViewerId,
            RoomId = integrationEvent.RoomId
        }, context.CancellationToken);
    }
}