using Overoom.Application.Abstractions.Film.Load.Exceptions;
using Overoom.Application.Abstractions.Film.Load.Interfaces;
using RestSharp;

namespace Overoom.Infrastructure.PhotoManager;

public class FilmPosterService : IFilmPosterService
{
    private readonly string _basePath;
    private readonly RestClient _client;

    public FilmPosterService(string basePath, HttpClient? client = null)
    {
        _basePath = Path.Combine(basePath, "Posters");
        _client = client == null ? new RestClient() : new RestClient(client, true);
        _client = new RestClient();
    }

    public async Task<Uri> SaveAsync(Uri url)
    {
        try
        {
            var stream = await _client.DownloadStreamAsync(new RestRequest(url));
            using var image = await Image.LoadAsync(stream ?? throw new NullReferenceException());
            image.Mutate(x => x.Resize(200, 500));
            var fileName = $"{Guid.NewGuid()}.jpg";
            await image.SaveAsync(Path.Combine(_basePath, fileName));
            return fileName;
        }
        catch (Exception ex)
        {
            throw new PosterSaveException(ex);
        }
    }

    public Task DeleteAsync(Uri uri)
    {
        File.Delete(Path.Combine(_basePath, fileName));
        return Task.CompletedTask;
    }
}