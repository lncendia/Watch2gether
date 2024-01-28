using Films.Application.Abstractions.Queries.FilmsApi;
using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.Interfaces;
using MediatR;
using Films.Domain.Films.Enums;

namespace Films.Application.Services.Queries.FilmsApi;

public class FindByTitleQueryHandler(
    IKpApiService kpApi,
    IVideoCdnApiService videoCdnApi,
    IBazonApiService bazonApiService) : IRequestHandler<FindByTitleQuery, FilmApiDto>
{
    public async Task<FilmApiDto> Handle(FindByTitleQuery request, CancellationToken cancellationToken)
    {
        var films = await kpApi.FindByTitleAsync(request.Title, token: cancellationToken);
        if (films.Count == 0) throw new NotImplementedException();
        var kpId = films.First().KpId;
        var film = await kpApi.GetAsync(kpId, cancellationToken);
        var staff = await kpApi.GetActorsAsync(kpId, cancellationToken);
        var seasons = film.Serial ? await kpApi.GetSeasonsAsync(kpId, cancellationToken) : null;
        var cdn = new List<CdnApiResponse>();
        try
        {
            cdn.Add(await bazonApiService.GetInfoAsync(kpId, cancellationToken));
            cdn.Add(await videoCdnApi.GetInfoAsync(kpId, cancellationToken));
        }
        catch
        {
            // ignored
        }

        return Map(film, staff, cdn, seasons);
    }

    private static FilmApiDto Map(FilmApiResponse filmApiResponse, FilmStaffApiResponse staffApiResponse, IReadOnlyCollection<CdnApiResponse> cdn,
        IReadOnlyCollection<SeasonApiResponse>? seasons)
    {
        return new FilmApiDto
        {
            Description = filmApiResponse.Description,
            ShortDescription = filmApiResponse.ShortDescription,
            Type = filmApiResponse.Serial ? FilmType.Serial : FilmType.Film,
            Title = filmApiResponse.Title,
            Year = filmApiResponse.Year,
            CountSeasons = seasons?.Count,
            CountEpisodes = seasons?.SelectMany(s => s.Episodes).Count(),
            Cdn = cdn,
            Countries = filmApiResponse.Countries,
            Actors = staffApiResponse.Actors,
            Directors = staffApiResponse.Directors,
            Genres = filmApiResponse.Genres,
            Screenwriters = staffApiResponse.ScreenWriters,
            RatingKp = filmApiResponse.RatingKp,
            RatingImdb = filmApiResponse.RatingImdb
        };
    }
}