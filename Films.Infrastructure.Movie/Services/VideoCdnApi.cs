using System.Net;
using Films.Application.Abstractions.MovieApi.DTOs;
using Films.Application.Abstractions.MovieApi.Exceptions;
using Films.Application.Abstractions.MovieApi.Interfaces;
using Films.Infrastructure.Movie.Abstractions;
using RestSharp;

namespace Films.Infrastructure.Movie.Services;

public class VideoCdnApi : IVideoCdnApiService, IDisposable
{
    private readonly RestClient _cdnClient;
    private readonly IVideoCdnResponseParser _parser;

    public VideoCdnApi(string cdnToken, IVideoCdnResponseParser parser)
    {
        _parser = parser;
        _cdnClient = new RestClient(options => options.MaxTimeout = 15000);
        _cdnClient.AddDefaultQueryParameter("api_token", cdnToken);
    }

    public async Task<CdnApiResponse> GetInfoAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest("https://videocdn.tv/api/short");
        request.AddQueryParameter("kinopoisk_id", kpId.ToString());
        var response = await _cdnClient.GetAsync(request, cancellationToken: token);
        if (response.StatusCode == HttpStatusCode.NotFound) throw new ApiNotFoundException();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ApiException($"The request was executed with the code {(int)response.StatusCode}",
                response.ErrorException);
        return _parser.Get(response.Content!);
    }

    public void Dispose()
    {
        _cdnClient.Dispose();
    }
}