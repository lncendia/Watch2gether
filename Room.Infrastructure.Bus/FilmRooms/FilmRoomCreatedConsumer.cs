using MassTransit;
using MassTransit.Mediator;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Room.Application.Abstractions.Commands.BaseRooms;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Domain.BaseRoom.ValueObjects;

namespace Room.Infrastructure.Bus.FilmRooms;

/// <summary>
/// Обработчик интеграционного события FilmRoomCreatedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class FilmRoomCreatedConsumer(IMediator mediator) : IConsumer<FilmRoomCreatedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<FilmRoomCreatedIntegrationEvent> context)
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
            Title = integrationEvent.Title,
            CdnUrl = integrationEvent.CdnUrl,
            IsSerial = integrationEvent.IsSerial
        }, context.CancellationToken);
    }
}