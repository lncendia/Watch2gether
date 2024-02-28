using Films.Application.Abstractions.Commands.Users;
using Films.Application.Abstractions.Common.Exceptions;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Users;

namespace Films.Infrastructure.Bus.Users;

/// <summary>
/// Обработчик интеграционного события UserChangedIntegrationEvent
/// </summary>
/// <param name="mediator">Медиатор</param>
public class UserChangedConsumer(IMediator mediator) : IConsumer<UserChangedIntegrationEvent>
{
    /// <summary>
    /// Метод обработчик 
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<UserChangedIntegrationEvent> context)
    {
        // Получаем данные события
        var integrationEvent = context.Message;

        try
        {
            // Отправляем команду на обработку события
            await mediator.Send(new ChangeUserCommand
            {
                Id = integrationEvent.Id,
                UserName = integrationEvent.Name,
                PhotoUrl = integrationEvent.PhotoUrl
            }, context.CancellationToken);
        }
        catch (UserNotFoundException)
        {
        }
    }
}