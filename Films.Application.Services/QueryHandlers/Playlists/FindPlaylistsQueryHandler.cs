using Films.Application.Abstractions.DTOs.Common;
using Films.Application.Abstractions.DTOs.Playlists;
using Films.Application.Abstractions.Queries.Playlists;
using Films.Application.Services.Extensions;
using Films.Application.Services.Mappers.Playlists;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Ordering;
using Films.Domain.Playlists;
using Films.Domain.Playlists.Ordering;
using Films.Domain.Playlists.Ordering.Visitor;
using Films.Domain.Playlists.Specifications;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Playlists;

public class FindPlaylistsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<FindPlaylistsQuery, ListDto<PlaylistDto>>
{
    public async Task<ListDto<PlaylistDto>> Handle(FindPlaylistsQuery request,
        CancellationToken cancellationToken)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;

        // Добавляем спецификации в соответствии с заданными параметрами поиска 
        if (!string.IsNullOrEmpty(request.Query))
            specification = specification.AddToSpecification(new PlaylistByNameSpecification(request.Query));

        if (!string.IsNullOrEmpty(request.Genre))
            specification = specification.AddToSpecification(new PlaylistByGenreSpecification(request.Genre));

        if (request.FilmId.HasValue)
            specification = specification.AddToSpecification(new PlaylistByFilmSpecification(request.FilmId.Value));

        var orderBy = new DescendingOrder<Playlist, IPlaylistSortingVisitor>(new PlaylistOrderByUpdateDate());

        var playlists =
            await unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy, request.Skip, request.Take);

        if (playlists.Count == 0) return new ListDto<PlaylistDto> { List = [], TotalCount = 0 };

        // Преобразуем фильмы в список DTO фильмов 
        return new ListDto<PlaylistDto>
        {
            List = playlists.Select(Mapper.Map).ToArray(),
            TotalCount = await unitOfWork.PlaylistRepository.Value.CountAsync(specification)
        };
    }
}