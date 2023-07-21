namespace Overoom.Application.Abstractions.Common.Interfaces;

public interface IUserThumbnailService
{
    Task<Uri> SaveAsync(Uri url);
    Task<Uri> SaveAsync(Stream stream);
    Task DeleteAsync(Uri uri);
}