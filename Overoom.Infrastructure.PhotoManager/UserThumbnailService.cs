using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Application.Abstractions.Users.Interfaces;

namespace Overoom.Infrastructure.PhotoManager;

public class UserThumbnailService : IUserThumbnailService
{
    private readonly string _rootPath;
    private readonly string _path;

    public UserThumbnailService(string rootPath, string path)
    {
        _rootPath = rootPath;
        _path = path;
    }


    public Task DeleteAsync(Uri uri)
    {
        File.Delete(Path.Combine(_rootPath, uri.ToString()));
        return Task.CompletedTask;
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
}