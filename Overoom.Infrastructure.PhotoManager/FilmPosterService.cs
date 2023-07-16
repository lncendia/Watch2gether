using Overoom.Application.Abstractions.FilmsManagement.Exceptions;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using RestSharp;

namespace Overoom.Infrastructure.PhotoManager;

public class FilmPosterService : IFilmPosterService
{
    private readonly string _rootPath;
    private readonly string _path;
    private readonly RestClient _client;

    public FilmPosterService(string rootPath, string path, HttpClient? client = null)
    {
        _rootPath = rootPath;
        _path = path;
        _client = client == null ? new RestClient() : new RestClient(client, true);
        _client = new RestClient();
    }

    public async Task<Uri> SaveAsync(Stream stream)
    {
        try
        {
            using var image = await Image.LoadAsync(stream);
            image.Mutate(x => x.Resize(200, 500));
            var fileName = $"{Guid.NewGuid()}.jpg";
            await image.SaveAsync(Path.Combine(Path.Combine(_rootPath, _path), fileName));
            return new Uri(Path.Combine(_path, fileName), UriKind.Relative);
        }
        catch (Exception ex)
        {
            throw new PosterSaveException(ex);
        }
    }

    public async Task<Uri> SaveAsync(Uri url)
    {
        try
        {
            var stream = await _client.DownloadStreamAsync(new RestRequest(url));
            using var image = await Image.LoadAsync(stream ?? throw new NullReferenceException());
            image.Mutate(x => x.Resize(200, 500));
            var fileName = $"{Guid.NewGuid()}.jpg";
            await image.SaveAsync(Path.Combine(Path.Combine(_rootPath, _path), fileName));
            return new Uri(Path.Combine(_path, fileName), UriKind.Relative);
        }
        catch (Exception ex)
        {
            throw new PosterSaveException(ex);
        }
    }

    public Task DeleteAsync(Uri uri)
    {
        File.Delete(Path.Combine(_rootPath, uri.ToString()));
        return Task.CompletedTask;
    }
}