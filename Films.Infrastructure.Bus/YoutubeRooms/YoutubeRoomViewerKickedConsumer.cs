using Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;

namespace Films.Infrastructure.Bus.YoutubeRooms;

/// <summary>
/// Обработчик интеграционного события YoutubeRoomCreatedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class YoutubeRoomViewerKickedConsumer(IMediator mediator) : IConsumer<YoutubeRoomViewerKickedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<YoutubeRoomViewerKickedIntegrationEvent> context)
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