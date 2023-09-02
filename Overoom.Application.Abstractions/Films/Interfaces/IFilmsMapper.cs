using Overoom.Application.Abstractions.Films.DTOs;
using Overoom.Domain.Films.Entities;

namespace Overoom.Application.Abstractions.Films.Interfaces;

public interface IFilmsMapper
{
    FilmDto Map(Film film);
}