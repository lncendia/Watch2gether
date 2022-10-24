using RestSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Overoom.Application.Abstractions.Exceptions.Users;
using Overoom.Application.Abstractions.Interfaces.Users;

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

    public async Task<string> SaveAsync(string url)
    {
        Stream stream;
        try
        {
            stream = await _client.DownloadStreamAsync(new RestRequest(url)) ?? throw new NullReferenceException();
        }
        catch (Exception e)
        {
            throw new ThumbnailSaveException(e);
        }

        return await SaveAsync(stream);
    }

    public async Task<string> SaveAsync(Stream stream)
    {
        try
        {
            using var image = await Image.LoadAsync(stream);
            image.Mutate(x => x.Resize(64, 64));
            var fileName = $"{Guid.NewGuid()}.jpg";
            await image.SaveAsync(Path.Combine(_basePath, fileName));
            return fileName;
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