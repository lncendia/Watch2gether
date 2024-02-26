using MassTransit;
using MassTransit.Mediator;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using Room.Application.Abstractions.Commands.BaseRooms;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Domain.BaseRoom.ValueObjects;

namespace Room.Infrastructure.Bus.YoutubeRooms;

/// <summary>
/// Обработчик интеграционного события YoutubeRoomViewerConnectedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class YoutubeRoomViewerConnectedConsumer(IMediator mediator) : IConsumer<YoutubeRoomViewerConnectedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<YoutubeRoomViewerConnectedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;
        
        // Отправляем команду на обработку события
        return mediator.Send(new ConnectCommand
        {
            RoomId = integrationEvent.RoomId,
            Viewer = new ViewerData
            {
                Id = integrationEvent.Viewer.Id,
                Nickname = integrationEvent.Viewer.Name,
                PhotoUrl = integrationEvent.Viewer.PhotoUrl,
                Allows = new Allows
                {
                    Beep = integrationEvent.Viewer.Beep,
                    Scream = integrationEvent.Viewer.Scream,
                    Change = integrationEvent.Viewer.Change
                }
            }
        }, context.CancellationToken);
    }
}