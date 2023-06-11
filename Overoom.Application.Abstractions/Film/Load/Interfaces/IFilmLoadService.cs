using Overoom.Application.Abstractions.Film.Load.DTOs;

namespace Overoom.Application.Abstractions.Film.Load.Interfaces;

public interface IFilmLoadService
{
    Task LoadAsync(FilmLoadDto film);
    Task ChangeAsync(FilmChangeDto film);
}