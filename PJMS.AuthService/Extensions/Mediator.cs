using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Abstractions.Commands;
using PJMS.AuthService.Services.Commands;
using PJMS.AuthService.Services.Commands.Authentication;

namespace PJMS.AuthService.Extensions;

///<summary>
/// Статический класс сервисов Mediator.
///</summary>
public static class Mediator
{
    ///<summary>
    /// Расширяющий метод для добавления сервисов Mediator в коллекцию служб.
    ///</summary>
    ///<param name="services">Коллекция служб.</param>
    public static void AddMediatorServices(this IServiceCollection services)
    {
        // Регистрация сервисов MediatR и обработчиков команд
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(AuthenticateUserByPasswordCommandHandler).Assembly);
        });
    }
}