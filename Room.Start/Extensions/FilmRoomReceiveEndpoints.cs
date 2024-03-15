using MassTransit;
using Overoom.IntegrationEvents.Rooms.FilmRooms;
using RabbitMQ.Client;
using Room.Infrastructure.Bus.FilmRooms;

namespace Room.Start.Extensions;

public static class FilmRoomReceiveEndpoints
{
    public static void AddFilmRoomReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext context, string serverId)
    {
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
        cfg.ReceiveEndpoint($"FilmRoomDeleted_{serverId}", re =>
        {
            // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
            re.ConfigureConsumeTopology = false;
            re.ConfigureConsumer<FilmRoomDeletedConsumer>(context);
            re.Bind<FilmRoomDeletedIntegrationEvent>(configurator =>
            {
                configurator.ExchangeType = ExchangeType.Direct;
                configurator.RoutingKey = serverId;
            });
        });
    }
}