using Films.Application.Abstractions.Queries.Playlists;
using Films.Application.Abstractions.Queries.Playlists.DTOs;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Ordering;
using Films.Domain.Playlists.Entities;
using Films.Domain.Playlists.Ordering;
using Films.Domain.Playlists.Ordering.Visitor;
using Films.Domain.Playlists.Specifications;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications;
using Films.Domain.Specifications.Abstractions;

namespace Films.Application.Services.Queries.Playlists;

public class FindPlaylistsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<FindPlaylistsQuery, IReadOnlyCollection<PlaylistDto>>
{
    public async Task<IReadOnlyCollection<PlaylistDto>> Handle(FindPlaylistsQuery request, CancellationToken cancellationToken)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;

        // Добавляем спецификации в соответствии с заданными параметрами поиска 
        if (!string.IsNullOrEmpty(request.Query))
            specification = AddToSpecification(specification, PlaylistByName(request.Query));

        if (!string.IsNullOrEmpty(request.Genre))
            specification = AddToSpecification(specification, PlaylistByGenre(request.Genre));

        var orderBy = new DescendingOrder<Playlist, IPlaylistSortingVisitor>(new PlaylistOrderByUpdateDate());

        var playlists =
            await unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy, request.Skip, request.Take);
        return playlists.Select(Map).ToArray();
    }

    private static PlaylistDto Map(Playlist playlist)
    {
        return new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Genres = playlist.Genres,
            Description = playlist.Description,
            PosterUrl = playlist.PosterUrl,
            Updated = playlist.Updated
        };
    }

    private static ISpecification<Playlist, IPlaylistSpecificationVisitor> PlaylistByGenre(string genre)
    {
        return new PlaylistByGenreSpecification(genre);
    }

    private static ISpecification<Playlist, IPlaylistSpecificationVisitor> PlaylistByName(string title)
    {
        return new PlaylistByNameSpecification(title);
    }


    private static ISpecification<T, TV> AddToSpecification<T, TV>(
        ISpecification<T, TV>? baseSpec, ISpecification<T, TV> newSpec) where TV : ISpecificationVisitor<TV, T>
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<T, TV>(baseSpec, newSpec);
    }
}