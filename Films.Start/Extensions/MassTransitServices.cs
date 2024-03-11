using Films.Infrastructure.Bus.FilmRooms;
using Films.Infrastructure.Bus.Users;
using Films.Infrastructure.Bus.YoutubeRooms;
using MassTransit;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using RabbitMQ.Client;

namespace Films.Start.Extensions;

/// <summary>
/// Статический класс для регистрации сервиса MassTransit в контейнере DI 
/// </summary>
public static class MassTransitServices
{
    /// <summary>
    /// Метод регистрирует сервис MassTransit в контейнере DI 
    /// </summary>
    /// <param name="services">Абстракция, которая представляет коллекцию сервисов (зависимостей),
    /// используемых в приложении.</param>
    /// <param name="configuration">Интерфейс, предоставляющий доступ к конфигурации приложения.</param>
    public static void AddMassTransitServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Получаем строку подключения к RabbitMq
        var rmq = configuration.GetRequiredValue<string>("ConnectionStrings:RabbitMq");

        //конфигурируем MassTransit
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<FilmRoomDeletedConsumer>();
            busConfigurator.AddConsumer<FilmRoomViewerKickedConsumer>();

            busConfigurator.AddConsumer<YoutubeRoomDeletedConsumer>();
            busConfigurator.AddConsumer<YoutubeRoomViewerKickedConsumer>();

            busConfigurator.AddConsumer<UserCreatedConsumer>();
            busConfigurator.AddConsumer<UserDataChangedConsumer>();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rmq);
                cfg.ConfigureEndpoints(context);
                
                // Конфигурируем обменники для событий комнат с фильмом
                cfg.Publish<FilmRoomCreatedIntegrationEvent>(configurator =>
                {
                    configurator.ExchangeType = ExchangeType.Direct;
                });
                cfg.Publish<FilmRoomViewerConnectedIntegrationEvent>(configurator =>
                {
                    configurator.ExchangeType = ExchangeType.Direct;
                });
                cfg.Publish<FilmRoomViewerLeavedIntegrationEvent>(configurator =>
                {
                    configurator.ExchangeType = ExchangeType.Direct;
                });

                // Конфигурируем обменники для событий комнат с ютубом
                cfg.Publish<YoutubeRoomCreatedIntegrationEvent>(configurator =>
                {
                    configurator.ExchangeType = ExchangeType.Direct;
                });
                cfg.Publish<YoutubeRoomViewerConnectedIntegrationEvent>(configurator =>
                {
                    configurator.ExchangeType = ExchangeType.Direct;
                });
                cfg.Publish<YoutubeRoomViewerLeavedIntegrationEvent>(configurator =>
                {
                    configurator.ExchangeType = ExchangeType.Direct;
                });
            });
        });
    }
}