using AuthService.Application.Abstractions.Abstractions.AppThumbnailStore;
using AuthService.Application.Abstractions.Exceptions;
using RestSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AuthService.Infrastructure.Storage;

public class UserThumbnailStore : IThumbnailStore, IDisposable
{
    private readonly string _rootPath;
    private readonly string _path;
    private readonly RestClient _client;

    public UserThumbnailStore(string rootPath, string path, HttpClient? client = null)
    {
        _rootPath = rootPath;
        _path = path;
        _client = client == null ? new RestClient() : new RestClient(client, true);
        _client = new RestClient();
    }
    
    public Task DeleteAsync(Uri uri)
    {
        File.Delete(Path.Combine(_rootPath, uri.ToString()));
        return Task.CompletedTask;
    }

    public async Task<Uri> SaveAsync(Uri url)
    {
        try
        {
            var stream = await _client.DownloadStreamAsync(new RestRequest(url));
            using var image = await Image.LoadAsync(stream ?? throw new NullReferenceException());
            image.Mutate(x => x.Resize(64, 64));
            var fileName = $"{Guid.NewGuid()}.jpg";
            await image.SaveAsync(Path.Combine(Path.Combine(_rootPath, _path), fileName));
            return new Uri(Path.Combine(_path, fileName), UriKind.Relative);
        }
        catch (Exception ex)
        {
            throw new ThumbnailSaveException(ex);
        }
    }

    public async Task<Uri> SaveAsync(Stream stream)
    {
        try
        {
            using var image = await Image.LoadAsync(stream);
            image.Mutate(x => x.Resize(64, 64));
            var fileName = $"{Guid.NewGuid()}.jpg";
            await image.SaveAsync(Path.Combine(Path.Combine(_rootPath, _path), fileName));
            return new Uri(Path.Combine(_path, fileName), UriKind.Relative);
        }
        catch (Exception ex)
        {
            throw new ThumbnailSaveException(ex);
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}