using Films.Application.Abstractions.Queries.FilmsApi;
using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.Interfaces;
using MediatR;

namespace Films.Application.Services.Queries.FilmsApi;

public class FindFilmByTitleQueryHandler(
    IKpApiService kpApi, IVideoCdnApiService videoCdnApi,
    IBazonApiService bazonApiService) : IRequestHandler<FindFilmByTitleQuery, FilmApiDto>
{
    public async Task<FilmApiDto> Handle(FindFilmByTitleQuery request, CancellationToken cancellationToken)
    {
        var films = await kpApi.FindByTitleAsync(request.Title, token: cancellationToken);
        if (films.Count == 0) throw new NotImplementedException();
        var kpId = films.First().KpId;
        var film = await kpApi.GetAsync(kpId, cancellationToken);
        var staff = await kpApi.GetActorsAsync(kpId, cancellationToken);
        var seasons = film.Serial ? await kpApi.GetSeasonsAsync(kpId, cancellationToken) : null;

        var bazon = await bazonApiService.GetInfoAsync(kpId, cancellationToken);
        var videoCdn = await videoCdnApi.GetInfoAsync(kpId, cancellationToken);

        return Mapper.Map(film, staff, videoCdn, bazon, seasons);
    }
}