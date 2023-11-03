using MediatR;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Playlists.Events;

namespace Overoom.Application.Services.PlaylistsManagement.EventHandlers;

public class FilmsCollectionUpdateEventHandler : INotificationHandler<FilmsCollectionUpdateEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public FilmsCollectionUpdateEventHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task Handle(FilmsCollectionUpdateEvent notification, CancellationToken cancellationToken)
    {
        var filmsSpec = new FilmByIdsSpecification(notification.Playlist.Films);
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(filmsSpec);
        var genres = films.SelectMany(x => x.FilmTags.Genres).GroupBy(g => g)
            .OrderByDescending(genre => genre.Count()).Select(x => x.Key).Take(5);
        notification.Playlist.UpdateGenres(genres);
        await _unitOfWork.PlaylistRepository.Value.UpdateAsync(notification.Playlist);
    }
}