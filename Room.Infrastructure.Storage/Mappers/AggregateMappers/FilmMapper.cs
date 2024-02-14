using Room.Domain.Films.Entities;
using Room.Domain.Films.ValueObjects;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.Film;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmMapper : IAggregateMapperUnit<Film, FilmModel>
{
    public Film Map(FilmModel model)
    {
        var film = new Film(model.Id)
        {
            Title = model.Title,
            Description = model.Description,
            Year = model.Year,
            PosterUrl = model.PosterUrl,
            Type = model.Type,
            CdnList = model.CdnList.Select(cdn => new Cdn
            {
                Name = cdn.Name,
                Url = cdn.Url
            }).ToArray()
        };

        return film;
    }
}