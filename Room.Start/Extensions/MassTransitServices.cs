using MassTransit;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using RabbitMQ.Client;
using Room.Infrastructure.Bus.FilmRooms;
using Room.Infrastructure.Bus.YoutubeRooms;

namespace Room.Start.Extensions;

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
        // Получаем строку подключения к RabbitMq
        var rmq = configuration.GetRequiredValue<string>("ConnectionStrings:RabbitMq");
        
        // Получаем идентификатор текущего сервера
        var serverId = configuration.GetRequiredValue<string>("Server:Id");

        // Конфигурируем MassTransit
        services.AddMassTransit(x =>
        {
            x.AddConsumer<FilmRoomCreatedConsumer>();
            x.AddConsumer<FilmRoomViewerConnectedConsumer>();
            x.AddConsumer<FilmRoomViewerLeavedConsumer>();

            x.AddConsumer<YoutubeRoomCreatedConsumer>();
            x.AddConsumer<YoutubeRoomViewerConnectedConsumer>();
            x.AddConsumer<YoutubeRoomViewerLeavedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rmq);
                
                // Конфигурируем эндпоинты для событий комнат с фильмом
                cfg.ReceiveEndpoint($"FilmRoomCreated_{serverId}", re =>
                {
                    // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
                    re.ConfigureConsumeTopology = false;
                    re.ConfigureConsumer<FilmRoomCreatedConsumer>(context);
                    re.Bind<FilmRoomCreatedIntegrationEvent>(configurator =>
                    {
                        configurator.ExchangeType = ExchangeType.Direct;
                        configurator.RoutingKey = serverId;
                    });
                });
                cfg.ReceiveEndpoint($"FilmRoomViewerConnected_{serverId}", re =>
                {
                    // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
                    re.ConfigureConsumeTopology = false;
                    re.ConfigureConsumer<FilmRoomViewerConnectedConsumer>(context);
                    re.Bind<FilmRoomViewerConnectedIntegrationEvent>(configurator =>
                    {
                        configurator.ExchangeType = ExchangeType.Direct;
                        configurator.RoutingKey = serverId;
                    });
                });
                cfg.ReceiveEndpoint($"FilmRoomViewerLeaved_{serverId}", re =>
                {
                    // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
                    re.ConfigureConsumeTopology = false;
                    re.ConfigureConsumer<FilmRoomViewerLeavedConsumer>(context);
                    re.Bind<FilmRoomViewerLeavedIntegrationEvent>(configurator =>
                    {
                        configurator.ExchangeType = ExchangeType.Direct;
                        configurator.RoutingKey = serverId;
                    });
                });
                
                // Конфигурируем эндпоинты для событий комнат с ютубом
                cfg.ReceiveEndpoint($"YoutubeRoomCreated_{serverId}", re =>
                {
                    // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
                    re.ConfigureConsumeTopology = false;
                    re.ConfigureConsumer<YoutubeRoomCreatedConsumer>(context);
                    re.Bind<YoutubeRoomCreatedIntegrationEvent>(configurator =>
                    {
                        configurator.ExchangeType = ExchangeType.Direct;
                        configurator.RoutingKey = serverId;
                    });
                });
                cfg.ReceiveEndpoint($"YoutubeRoomViewerConnected_{serverId}", re =>
                {
                    // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
                    re.ConfigureConsumeTopology = false;
                    re.ConfigureConsumer<YoutubeRoomViewerConnectedConsumer>(context);
                    re.Bind<YoutubeRoomViewerConnectedIntegrationEvent>(configurator =>
                    {
                        configurator.ExchangeType = ExchangeType.Direct;
                        configurator.RoutingKey = serverId;
                    });
                });
                cfg.ReceiveEndpoint($"YoutubeRoomViewerLeaved_{serverId}", re =>
                {
                    // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
                    re.ConfigureConsumeTopology = false;
                    re.ConfigureConsumer<YoutubeRoomViewerLeavedConsumer>(context);
                    re.Bind<YoutubeRoomViewerLeavedIntegrationEvent>(configurator =>
                    {
                        configurator.ExchangeType = ExchangeType.Direct;
                        configurator.RoutingKey = serverId;
                    });
                });
            });
        });
    }
}