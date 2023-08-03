using MediatR;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Playlists.Events;

namespace Overoom.Application.Services.PlaylistsManagement.EventHandlers;

public class FilmsCollectionUpdateEventHandler : INotificationHandler<FilmsCollectionUpdateEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public FilmsCollectionUpdateEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(FilmsCollectionUpdateEvent notification, CancellationToken cancellationToken)
    {
       
    }
}