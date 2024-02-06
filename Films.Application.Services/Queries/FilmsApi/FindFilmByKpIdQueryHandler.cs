using Films.Application.Abstractions.Queries.FilmsApi;
using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.Interfaces;
using MediatR;

namespace Films.Application.Services.Queries.FilmsApi;

public class FindFilmByKpIdQueryHandler(IKpApiService kpApi, IVideoCdnApiService videoCdnApi)
    : IRequestHandler<FindFilmByKpIdQuery, FilmApiDto>
{
    public async Task<FilmApiDto> Handle(FindFilmByKpIdQuery request, CancellationToken cancellationToken)
    {
        var film = await kpApi.GetAsync(request.Id, cancellationToken);
        var staff = await kpApi.GetActorsAsync(request.Id, cancellationToken);
        var seasons = film.Serial ? await kpApi.GetSeasonsAsync(request.Id, cancellationToken) : null;
        
        var videoCdn = await videoCdnApi.GetInfoAsync(request.Id, cancellationToken);

        return Mapper.Map(film, staff, videoCdn, seasons);
    }
}