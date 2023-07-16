using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Films.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Film;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmModelMapper : IModelMapperUnit<FilmModel, Film>
{
    private readonly ApplicationDbContext _context;

    public FilmModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<FilmModel> MapAsync(Film entity)
    {
        var film = await _context.Films.FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (film != null)
        {
            await _context.Entry(film).Collection(x => x.Countries).LoadAsync();
            await _context.Entry(film).Collection(x => x.Directors).LoadAsync();
            await _context.Entry(film).Collection(x => x.Actors).LoadAsync();
            await _context.Entry(film).Collection(x => x.Genres).LoadAsync();
            await _context.Entry(film).Collection(x => x.ScreenWriters).LoadAsync();
            await _context.Entry(film).Collection(x => x.CdnList).Query().Include(x => x.Voices).LoadAsync();
        }
        else
        {
            film = new FilmModel
            {
                Id = entity.Id,
                Type = entity.Type,
                Name = entity.Name,
                NameNormalized = entity.Name.ToUpper(),
                Year = entity.Year
            };
            film.Countries.AddRange(entity.FilmTags.Countries.Select(x => new CountryModel
                { Name = x, NameNormalized = x.ToUpper() }));
            film.Directors.AddRange(entity.FilmTags.Directors.Select(x => new DirectorModel
                { Name = x, NameNormalized = x.ToUpper() }));
            film.Genres.AddRange(entity.FilmTags.Genres.Select(x => new GenreModel
                { Name = x, NameNormalized = x.ToUpper() }));
            film.ScreenWriters.AddRange(entity.FilmTags.Screenwriters.Select(x => new ScreenWriterModel
                { Name = x, NameNormalized = x.ToUpper() }));
            film.Actors.AddRange(entity.FilmTags.Actors.Select(x => new ActorModel
                { Name = x.ActorName, Description = x.ActorDescription, NameNormalized = x.ActorName.ToUpper() }));
        }

        film.PosterUri = entity.PosterUri;
        film.Description = entity.Description;
        film.ShortDescription = entity.ShortDescription;
        film.Rating = entity.Rating;
        film.UserRating = entity.UserRating;
        film.CountSeasons = entity.CountSeasons;
        film.CountEpisodes = entity.CountEpisodes;
        film.UserRatingsCount = entity.UserRatingsCount;


        _context.FilmCdn.RemoveRange(film.CdnList.Where(x => entity.CdnList.All(cdn => cdn.Type != x.Type)));

        foreach (var cdn in entity.CdnList)
        {
            var cdnModel = film.CdnList.FirstOrDefault(x => x.Type == cdn.Type) ?? new CdnModel();

            cdnModel.Uri = cdn.Uri;
            cdnModel.Quality = cdn.Quality;

            var newVoices = cdn.Voices.Where(x => cdnModel.Voices.All(m => m.InfoNormalized != x.ToUpper()));
            cdnModel.Voices.AddRange(newVoices.Select(x => new VoiceModel { Info = x, InfoNormalized = x.ToUpper() }));
            _context.CdnVoices.RemoveRange(cdnModel.Voices.Where(x =>
                cdn.Voices.All(m => m.ToUpper() != x.InfoNormalized)));
            if (cdnModel.Id == default) film.CdnList.Add(cdnModel);
        }

        return film;
    }
}