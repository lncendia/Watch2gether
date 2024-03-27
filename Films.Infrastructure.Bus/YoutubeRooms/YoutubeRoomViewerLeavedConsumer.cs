using Films.Application.Abstractions.Commands.YoutubeRooms;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;

namespace Films.Infrastructure.Bus.YoutubeRooms;

/// <summary>
/// Обработчик интеграционного события FilmRoomViewerLeavedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class YoutubeRoomViewerLeavedConsumer(IMediator mediator) : IConsumer<YoutubeRoomViewerLeavedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<YoutubeRoomViewerLeavedIntegrationEvent> context)
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