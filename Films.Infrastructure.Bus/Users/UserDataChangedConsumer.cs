using Films.Application.Abstractions.Commands.Profile;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Users;

namespace Films.Infrastructure.Bus.Users;

/// <summary>
/// Обработчик интеграционного события UserDataChangedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class UserDataChangedConsumer(IMediator mediator) : IConsumer<UserDataChangedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<UserDataChangedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        // Отправляем команду на обработку события
        await mediator.Send(new ChangeUserCommand
        {
            Id = integrationEvent.Id,
            UserName = integrationEvent.Name,
            PhotoUrl = integrationEvent.PhotoUrl
        }, context.CancellationToken);
    }
}