namespace Overoom.Application.Abstractions.User.Interfaces;

public interface IUserThumbnailService
{
    Task<string> SaveAsync(string url);
    Task<string> SaveAsync(Stream stream);
    Task DeleteAsync(string fileName);
}