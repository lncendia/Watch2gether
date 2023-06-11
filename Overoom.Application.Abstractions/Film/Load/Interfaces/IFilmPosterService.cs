namespace Overoom.Application.Abstractions.Film.Load.Interfaces;

public interface IFilmPosterService
{
    Task<Uri> SaveAsync(Uri url);
    Task DeleteAsync(Uri uri);
}