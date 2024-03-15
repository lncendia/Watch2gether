using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using Room.Application.Abstractions.Commands.YoutubeRooms;

namespace Room.Infrastructure.Bus.YoutubeRooms;

/// <summary>
/// Обработчик интеграционного события YoutubeRoomViewerLeavedIntegrationEvent
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
        return mediator.Send(new LeaveCommand
        {
            RoomId = integrationEvent.RoomId,
            ViewerId = integrationEvent.ViewerId
        }, context.CancellationToken);
    }
}