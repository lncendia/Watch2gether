using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using Room.Application.Abstractions.Commands.YoutubeRooms;

namespace Room.Infrastructure.Bus.YoutubeRooms;

/// <summary>
/// Обработчик интеграционного события YoutubeRoomDeletedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class YoutubeRoomDeletedConsumer(IMediator mediator) : IConsumer<YoutubeRoomDeletedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public Task Consume(ConsumeContext<YoutubeRoomDeletedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        return mediator.Send(new DeleteRoomCommand { Id = integrationEvent.Id }, context.CancellationToken);
    }
}