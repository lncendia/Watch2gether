﻿using Microsoft.EntityFrameworkCore;
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
                Year = entity.Year
            };
            film.Countries.AddRange(entity.FilmTags.Countries.Select(x => new CountryModel { Name = x }));
            film.Directors.AddRange(entity.FilmTags.Directors.Select(x => new DirectorModel { Name = x }));
            film.Genres.AddRange(entity.FilmTags.Genres.Select(x => new GenreModel { Name = x }));
            film.ScreenWriters.AddRange(entity.FilmTags.Screenwriters.Select(x => new ScreenWriterModel { Name = x }));
            film.Actors.AddRange(entity.FilmTags.Actors.Select(x => new ActorModel
                { Name = x.ActorName, Description = x.ActorDescription }));
        }

        film.PosterUri = entity.PosterUri;
        film.Description = entity.Description;
        film.ShortDescription = entity.ShortDescription;
        film.RatingKp = entity.RatingKp;
        film.UserRating = entity.UserRating;
        film.CountSeasons = entity.CountSeasons;
        film.CountEpisodes = entity.CountEpisodes;


        film.CdnList.RemoveAll(x => entity.CdnList.All(cdn => cdn.Type != x.Type));

        foreach (var cdn in entity.CdnList)
        {
            var cdnModel = film.CdnList.FirstOrDefault(x => x.Type == cdn.Type) ?? new CdnModel();

            cdnModel.Uri = cdn.Uri;
            cdnModel.Quality = cdn.Quality;

            var newVoices = cdn.Voices.Where(x => cdnModel.Voices.All(m => m.Info != x));
            cdnModel.Voices.AddRange(newVoices.Select(x => new VoiceModel { Info = x }));
            cdnModel.Voices.RemoveAll(x => cdn.Voices.All(m => m != x.Info));
            if (cdnModel.Id == default) film.CdnList.Add(cdnModel);
        }

        return film;
    }
}