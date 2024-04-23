using Films.Application.Abstractions.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.YoutubeRooms.Events;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using MassTransit;
using MediatR;
using Overoom.IntegrationEvents.Rooms.BaseRooms;
using Overoom.IntegrationEvents.Rooms.YoutubeRooms;

namespace Films.Application.Services.EventHandlers.YoutubeRooms;

public class YoutubeRoomCreatedEventHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    : INotificationHandler<YoutubeRoomCreatedDomainEvent>
{
    public async Task Handle(YoutubeRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var filmRoomsSpecification = new YoutubeRoomByUserSpecification(notification.Owner.Id);
        var filmRoomsCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(filmRoomsSpecification);

        var youtubeRoomsSpecification = new YoutubeRoomByUserSpecification(notification.Owner.Id);
        var youtubeRoomsCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(youtubeRoomsSpecification);

        if (filmRoomsCount + youtubeRoomsCount >= 3) throw new MaxNumberRoomsReachedException();
        
        await publishEndpoint.Publish(new YoutubeRoomCreatedIntegrationEvent
        {
            Id = notification.Room.Id,
            VideoAccess = notification.Room.VideoAccess,
            Owner = new Viewer
            {
                Id = notification.Owner.Id,
                PhotoUrl = notification.Owner.PhotoUrl,
                Name = notification.Owner.Username,
                Beep = notification.Owner.Allows.Beep,
                Scream = notification.Owner.Allows.Scream,
                Change = notification.Owner.Allows.Change
            }
        }, context => context.SetRoutingKey(notification.Room.ServerId.ToString()), cancellationToken);
    }
}