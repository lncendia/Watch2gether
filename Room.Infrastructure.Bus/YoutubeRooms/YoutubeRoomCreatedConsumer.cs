using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using Room.Application.Abstractions.Commands.Rooms;
using Room.Application.Abstractions.Commands.YoutubeRooms;
using Room.Domain.Rooms.Rooms.ValueObjects;

namespace Room.Infrastructure.Bus.YoutubeRooms;

/// <summary>
/// Обработчик интеграционного события YoutubeRoomCreatedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class YoutubeRoomCreatedConsumer(IMediator mediator) : IConsumer<YoutubeRoomCreatedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<YoutubeRoomCreatedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        return mediator.Send(new CreateRoomCommand
        {
            Id = integrationEvent.Id,
            Owner = new ViewerData
            {
                Id = integrationEvent.Owner.Id,
                Nickname = integrationEvent.Owner.Name,
                PhotoUrl = integrationEvent.Owner.PhotoUrl,
                Allows = new Allows
                {
                    Beep = integrationEvent.Owner.Beep,
                    Scream = integrationEvent.Owner.Scream,
                    Change = integrationEvent.Owner.Change
                }
            },
            VideoAccess = integrationEvent.VideoAccess
        }, context.CancellationToken);
    }
}