using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Commands.Rooms;
using Room.Domain.Rooms.Rooms.ValueObjects;

namespace Room.Infrastructure.Bus.FilmRooms;

/// <summary>
/// Обработчик интеграционного события FilmRoomViewerConnectedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class FilmRoomViewerConnectedConsumer(IMediator mediator) : IConsumer<FilmRoomViewerConnectedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<FilmRoomViewerConnectedIntegrationEvent> context)
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