namespace AuthService.Application.Abstractions.Abstractions.AppThumbnailStore;

public interface IThumbnailStore
{
    Task<Uri> SaveAsync(Uri url);
    Task<Uri> SaveAsync(Stream stream);
    Task DeleteAsync(Uri uri);
}