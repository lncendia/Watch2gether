using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Commands.Rooms;
using Room.Domain.Rooms.FilmRooms.ValueObjects;
using Room.Domain.Rooms.Rooms.ValueObjects;

namespace Room.Infrastructure.Bus.FilmRooms;

/// <summary>
/// Обработчик интеграционного события FilmRoomCreatedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class FilmRoomCreatedConsumer(ISender mediator) : IConsumer<FilmRoomCreatedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<FilmRoomCreatedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        await mediator.Send(new CreateRoomCommand
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
            Cdn = new Cdn
            {
                Name = integrationEvent.CdnName,
                Url = integrationEvent.CdnUrl
            },
            IsSerial = integrationEvent.IsSerial
        }, context.CancellationToken);

        await context.RespondAsync(new FilmRoomAcceptedIntegrationEvent { Id = integrationEvent.Id });
    }
}