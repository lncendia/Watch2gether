namespace Overoom.Application.Abstractions.Users.Interfaces;

public interface IUserThumbnailService
{
    Task<Uri> SaveAsync(Stream stream);
    Task DeleteAsync(Uri uri);
}