using MassTransit;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;
using RabbitMQ.Client;
using Room.Infrastructure.Bus.YoutubeRooms;

namespace Room.Start.Extensions;

public static class YoutubeRoomReceiveEndpoints
{
    public static void AddYoutubeRoomReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext context, string serverId)
    {
        // Конфигурируем эндпоинты для событий комнат с фильмом
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
        cfg.ReceiveEndpoint($"YoutubeRoomDeleted_{serverId}", re =>
        {
            // Отключаем конфигурацию по умолчанию, так как мы конфигурируем обменник вручную ниже в методе Bind
            re.ConfigureConsumeTopology = false;
            re.ConfigureConsumer<YoutubeRoomDeletedConsumer>(context);
            re.Bind<YoutubeRoomDeletedIntegrationEvent>(configurator =>
            {
                configurator.ExchangeType = ExchangeType.Direct;
                configurator.RoutingKey = serverId;
            });
        });
    }
}