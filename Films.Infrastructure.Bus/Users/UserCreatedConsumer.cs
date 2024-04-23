using Films.Application.Abstractions.Commands.Profile;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Users;

namespace Films.Infrastructure.Bus.Users;

/// <summary>
/// Обработчик интеграционного события UserCreatedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class UserCreatedConsumer(IMediator mediator) : IConsumer<UserCreatedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        await mediator.Send(new AddUserCommand
        {
            Id = integrationEvent.Id,
            UserName = integrationEvent.Name,
            PhotoUrl = integrationEvent.PhotoUrl
        }, context.CancellationToken);
    }
}