using Overoom.Application.Abstractions.Films.Load.DTOs;

namespace Overoom.Application.Abstractions.Films.Load.Interfaces;

public interface IFilmLoadService
{
    Task LoadAsync(FilmLoadDto film);
    Task ChangeAsync(FilmChangeDto film);
}