namespace Overoom.Application.Abstractions.FilmsManagement.Interfaces;

public interface IFilmPosterService
{
    Task<Uri> SaveAsync(Stream stream);
    Task<Uri> SaveAsync(Uri url);
    Task DeleteAsync(Uri uri);
}