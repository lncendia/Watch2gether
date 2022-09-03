namespace Watch2gether.Application.Abstractions.Interfaces.Users;

public interface IUserThumbnailService
{
    Task<string> SaveAsync(string url);
    Task<string> SaveAsync(Stream stream);
    Task DeleteAsync(string fileName);
}