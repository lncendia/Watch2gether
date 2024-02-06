using Films.Application.Abstractions.Queries.Playlists;
using Films.Application.Abstractions.Queries.Playlists.DTOs;
using Films.Application.Services.Common;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Ordering;
using Films.Domain.Playlists.Entities;
using Films.Domain.Playlists.Ordering;
using Films.Domain.Playlists.Ordering.Visitor;
using Films.Domain.Playlists.Specifications;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Application.Services.Queries.Playlists;

public class FindPlaylistsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<FindPlaylistsQuery, (IReadOnlyCollection<PlaylistDto> playlists, int count)>
{
    public async Task<(IReadOnlyCollection<PlaylistDto> playlists, int count)> Handle(FindPlaylistsQuery request,
        CancellationToken cancellationToken)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;

        // Добавляем спецификации в соответствии с заданными параметрами поиска 
        if (!string.IsNullOrEmpty(request.Query)) specification = specification.AddToSpecification(new PlaylistByNameSpecification(request.Query));

        if (!string.IsNullOrEmpty(request.Genre)) specification = specification.AddToSpecification(new PlaylistByGenreSpecification(request.Genre));

        var orderBy = new DescendingOrder<Playlist, IPlaylistSortingVisitor>(new PlaylistOrderByUpdateDate());

        var playlists = await unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy, request.Skip, request.Take);
        
        if (playlists.Count == 0) return ([], 0);

        var count = await unitOfWork.PlaylistRepository.Value.CountAsync(specification);

        // Преобразуем фильмы в список DTO фильмов 
        return (playlists.Select(Map).ToArray(), count);
    }

    private static PlaylistDto Map(Playlist playlist) => new()
    {
        Id = playlist.Id,
        Name = playlist.Name,
        Genres = playlist.Genres,
        Description = playlist.Description,
        PosterUrl = playlist.PosterUrl,
        Updated = playlist.Updated
    };
}