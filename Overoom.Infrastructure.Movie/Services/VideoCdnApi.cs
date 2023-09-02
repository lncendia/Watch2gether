using System.Net;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Application.Abstractions.MovieApi.Exceptions;
using Overoom.Application.Abstractions.MovieApi.Interfaces;
using Overoom.Infrastructure.Movie.Abstractions;
using RestSharp;

namespace Overoom.Infrastructure.Movie.Services;

public class VideoCdnApi : IVideoCdnApiService
{
    private readonly RestClient _cdnClient;
    private readonly IVideoCdnResponseParser _parser;

    public VideoCdnApi(string cdnToken, IVideoCdnResponseParser parser, HttpClient? client = null)
    {
        _parser = parser;
        _cdnClient = client != null
            ? new RestClient(client, true, options => options.MaxTimeout = 15000)
            : new RestClient(options => options.MaxTimeout = 15000);
        _cdnClient.AddDefaultQueryParameter("api_token", cdnToken);
    }

    public async Task<Cdn> GetInfoAsync(long kpId, CancellationToken token = default)
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
}