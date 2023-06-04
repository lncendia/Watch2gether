namespace Overoom.Application.Abstractions.Film.Interfaces;

public interface IFilmPosterService
{
    Task<string> SaveAsync(string url);
    Task DeleteAsync(string fileName);
}