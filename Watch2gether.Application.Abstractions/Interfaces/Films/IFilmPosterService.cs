namespace Watch2gether.Application.Abstractions.Interfaces.Films;

public interface IFilmPosterService
{
    Task<string> SaveAsync(string url);
    Task DeleteAsync(string fileName);
}