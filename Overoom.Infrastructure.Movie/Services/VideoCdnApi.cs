using System.Net;
using Overoom.Application.Abstractions.Kinopoisk.DTOs;
using Overoom.Application.Abstractions.Kinopoisk.Exceptions;
using Overoom.Application.Abstractions.Kinopoisk.Interfaces;
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
        _cdnClient = client != null ? new RestClient(client, true) : new RestClient();
        _cdnClient.AddDefaultQueryParameter("api_token", cdnToken);
    }

    public async Task<Cdn> GetInfoAsync(long kpId)
    {
        var request = new RestRequest("https://videocdn.tv/api/short");
        request.AddQueryParameter("kinopoisk_id", kpId.ToString());
        var response = await _cdnClient.GetAsync(request);
        if (response.StatusCode == HttpStatusCode.NotFound) throw new ApiNotFoundException();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ApiException($"The request was executed with the code {(int)response.StatusCode}",
                response.ErrorException);
        return _parser.Get(response.Content!);
    }
}