using System.Net;
using Films.Application.Abstractions.Services.MovieApi.DTOs;
using Films.Application.Abstractions.Services.MovieApi.Exceptions;
using Films.Application.Abstractions.Services.MovieApi.Interfaces;
using Films.Infrastructure.Movie.Abstractions;
using RestSharp;

namespace Films.Infrastructure.Movie.Services;

public class BazonApi : IBazonApiService
{
    private readonly RestClient _cdnClient;
    private readonly IBazonResponseParser _parser;

    public BazonApi(string cdnToken, IBazonResponseParser parser, HttpClient? client = null)
    {
        _parser = parser;
        _cdnClient = client != null
            ? new RestClient(client, true, options => options.MaxTimeout = 15000)
            : new RestClient(options => options.MaxTimeout = 15000);
        _cdnClient.AddDefaultQueryParameter("token", cdnToken);
    }

    public async Task<CdnApiResponse> GetInfoAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest("https://bazon.cc/api/search");
        request.AddQueryParameter("kp", kpId.ToString());
        var response = await _cdnClient.GetAsync(request, cancellationToken: token);
        if (response.StatusCode == HttpStatusCode.NotFound) throw new ApiNotFoundException();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ApiException($"The request was executed with the code {(int)response.StatusCode}",
                response.ErrorException);
        return _parser.Get(response.Content!);
    }
}