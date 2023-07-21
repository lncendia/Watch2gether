using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.Domain.Films.Entities;

namespace Overoom.Application.Abstractions.FilmsManagement.Interfaces;

public interface IFilmManagementMapper
{
    FilmDto MapGet(Film film);
    FilmShortDto MapShort(Film film);
}