using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Overoom.Application.Abstractions.DTO.Films.FilmInfoGetter;
using Overoom.Application.Abstractions.Exceptions.Films;
using Overoom.Application.Abstractions.Interfaces.Films;
using Overoom.Infrastructure.MovieDownloader.Converters;
using Overoom.Infrastructure.MovieDownloader.Enums;
using Overoom.Infrastructure.MovieDownloader.Models;

namespace Overoom.Infrastructure.MovieDownloader;

public class FilmGetterService : IFilmInfoGetterService
{
    private readonly RestClient _cdnClient, _kinopoiskClient;

    public FilmGetterService(string cdnToken, string kinopoiskToken, HttpClient? client = null)
    {
        if (client != null)
        {
            _cdnClient = new RestClient(client, true);
            _kinopoiskClient = new RestClient(client, true);
        }
        else
        {
            _cdnClient = new RestClient();
            _kinopoiskClient = new RestClient();
        }

        _cdnClient.AddDefaultQueryParameter("api_token", cdnToken);
        _kinopoiskClient.AddDefaultHeader("X-API-KEY", kinopoiskToken);

        _cdnClient.UseNewtonsoftJson(new JsonSerializerSettings
            {Converters = {new DateTimeConverter(), new FilmTypeConverter()}});
        _kinopoiskClient.UseNewtonsoftJson(new JsonSerializerSettings
            {Converters = {new ListObjectsConverter(), new ProfessionConverter()}});
    }


    public async Task<GetterResultDto> GetFilmsAsync(string? title, int page, int pageSize)
    {
        var request = new RestRequest("https://videocdn.tv/api/short");
        if (!string.IsNullOrEmpty(title)) request.AddQueryParameter("title", title);
        request.AddHeader("API_TOKEN", "6oDZugvTXZogUnTodylqzeEP7c4lmnkd");
        request.AddQueryParameter("page", page.ToString());
        request.AddQueryParameter("limit", pageSize.ToString());
        SearchResult result;
        try
        {
            result = (await _cdnClient.GetAsync<SearchResult>(request))!;
        }
        catch (Exception e)
        {
            throw new RequestException(e);
        }

        if (!result.Success) throw new RequestException();
        result.Films.RemoveAll(x => string.IsNullOrEmpty(x.KpId));
        return Map(result);
    }

    public async Task<FilmFullInfoDto> GetFilmFromVideoCdnIdAsync(int id)
    {
        FilmData film;
        (FilmFullData filmData, List<ActorsData> actorsData) filmData;
        try
        {
            film = await GetFilmAsync(id);
            filmData = await GetFilmDataAsync(film.KpId!);
        }
        catch (Exception e)
        {
            throw new RequestException(e);
        }

        return Map(film, filmData.filmData, filmData.actorsData);
    }

    private async Task<(FilmFullData filmData, List<ActorsData> actorsData)> GetFilmDataAsync(string kpId)
    {
        var request = new RestRequest($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{kpId}");
        var request2 = new RestRequest("https://kinopoiskapiunofficial.tech/api/v1/staff");
        request2.AddQueryParameter("filmId", kpId);
        var t1 = _kinopoiskClient.GetAsync<FilmFullData>(request);
        var t2 = _kinopoiskClient.GetAsync<List<ActorsData>>(request2);
        await Task.WhenAll(t1, t2);
        return (t1.Result!, t2.Result!);
    }


    private async Task<FilmData> GetFilmAsync(int id)
    {
        var request = new RestRequest($"https://videocdn.tv/api/short");
        request.AddQueryParameter("kinopoisk_id", id);
        SearchResult result;
        try
        {
            result = (await _cdnClient.GetAsync<SearchResult>(request))!;
        }
        catch (Exception e)
        {
            throw new RequestException(e);
        }

        if (!result.Success || !result.Films.Any()) throw new RequestException();
        return result.Films.First();
    }

    private static GetterResultDto Map(SearchResult result)
    {
        var films = result.Films.Select(x => new FilmInfoDto(x.KpId!, x.Title, x.Date.Year, x.FilmType));
        return new GetterResultDto(films.ToList(), result.Count, result.LastPage, result.Page);
    }

    private static FilmFullInfoDto Map(FilmData data, FilmFullData filmFullData,
        IReadOnlyCollection<ActorsData> actorsData)
    {
        var directors = actorsData.Where(x => x.Profession == Profession.Director)
            .Select(x => GetName(x.Name, x.NameEn));
        var actors = actorsData.Where(x => x.Profession == Profession.Actor)
            .Select(x => (GetName(x.Name, x.NameEn), x.Description!));
        var screenwriters = actorsData.Where(x => x.Profession == Profession.Writer)
            .Select(x => GetName(x.Name, x.NameEn));

        double rating;
        if (filmFullData.RatingKinopoisk.HasValue) rating = filmFullData.RatingKinopoisk.Value;
        else if (filmFullData.RatingImdb.HasValue) rating = filmFullData.RatingImdb.Value;
        else rating = 0;

        return new FilmFullInfoDto(data.Title, filmFullData.Description, filmFullData.ShortDescription,
            rating, data.Date, data.FilmType, new Uri("https:" + data.Url), filmFullData.Genres, actors,
            filmFullData.Countries, directors, screenwriters, filmFullData.PosterUrl, data.SeasonsCount,
            data.EpisodesCount);
    }

    private static string GetName(string? name, string? nameEn) =>
        (string.IsNullOrEmpty(name) ? nameEn : name) ?? string.Empty;
}