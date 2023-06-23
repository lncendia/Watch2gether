using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Application.Abstractions.Users.Interfaces;
using RestSharp;

namespace Overoom.Infrastructure.PhotoManager;

public class UserThumbnailService : IUserThumbnailService
{
    private readonly string _basePath;
    private readonly RestClient _client;

    public UserThumbnailService(string basePath, HttpClient? client = null)
    {
        _basePath = Path.Combine(basePath, "Avatars");
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
            return new Uri(fileName, UriKind.Relative);
        }
        catch (Exception ex)
        {
            throw new ThumbnailSaveException(ex);
        }
    }


    public Task DeleteAsync(Uri uri)
    {
        File.Delete(Path.Combine(_basePath, uri.ToString()));
        return Task.CompletedTask;
    }

    public async Task<Uri> SaveAsync(Stream stream)
    {
        try
        {
            using var image = await Image.LoadAsync(stream);
            image.Mutate(x => x.Resize(64, 64));
            var fileName = $"{Guid.NewGuid()}.jpg";
            await image.SaveAsync(Path.Combine(_basePath, fileName));
            return new Uri(fileName, UriKind.Relative);
        }
        catch (Exception ex)
        {
            throw new ThumbnailSaveException(ex);
        }
    }

    public Task DeleteAsync(string fileName)
    {
        File.Delete(Path.Combine(_basePath, fileName));
        return Task.CompletedTask;
    }
}