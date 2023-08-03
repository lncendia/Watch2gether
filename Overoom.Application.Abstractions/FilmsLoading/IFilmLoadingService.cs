namespace Overoom.Application.Abstractions.FilmsLoading;

public interface IFilmAutoLoader
{
    Task LoadAsync(long id, CancellationToken token = default);
}