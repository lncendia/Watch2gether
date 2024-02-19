using Films.Application.Abstractions.Common.Exceptions;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.YoutubeRooms.Events;
using Films.Domain.Rooms.YoutubeRooms.Specifications;
using MassTransit;
using MediatR;

namespace Films.Application.Services.EventHandlers;

public class YoutubeRoomCreatedEventHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : INotificationHandler<YoutubeRoomCreatedDomainEvent>
{
    public async Task Handle(YoutubeRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var filmRoomsSpecification = new YoutubeRoomByUserSpecification(notification.Owner.Id);
        var filmRoomsCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(filmRoomsSpecification);

        var youtubeRoomsSpecification = new YoutubeRoomByUserSpecification(notification.Owner.Id);
        var youtubeRoomsCount = await unitOfWork.YoutubeRoomRepository.Value.CountAsync(youtubeRoomsSpecification);

        if (filmRoomsCount + youtubeRoomsCount >= 3) throw new MaxNumberRoomsReachedException();

        publishEndpoint.Publish();
    }
}