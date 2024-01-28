using Films.Application.Abstractions.FilmsManagement.DTOs;
using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Infrastructure.Web.Contracts.FilmManagement;
using Films.Infrastructure.Web.Contracts.FilmManagement.Load;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.FilmManagement;

namespace Films.Infrastructure.Web.Mappers;

public class FilmManagementMapper : IFilmManagementMapper
{
    public ChangeFilmInputModel Map(FilmDto dto)
    {
        var cdns = dto.CdnList.Select(x => new Contracts.FilmManagement.Change.CdnParameters
        {
            Quality = x.Quality, Type = x.Type, Uri = x.Uri.ToString(),
            Voices = x.Voices.Select(v => new Contracts.FilmManagement.Change.VoiceParameters { Name = v }).ToList()
        }).ToList();
        return new ChangeFilmInputModel
        {
            Id = dto.FilmId, CountEpisodes = dto.CountEpisodes, CountSeasons = dto.CountSeasons,
            Description = dto.Description, Rating = dto.Rating,
            ShortDescription = dto.ShortDescription, Cdns = cdns, Name = dto.Title
        };
    }

    public ChangeDto Map(ChangeFilmInputModel filmInputModel)
    {
        var uri = filmInputModel.NewPosterUri == null ? null : new Uri(filmInputModel.NewPosterUri);
        var cdns = filmInputModel.Cdns
            .Select(x => new CdnDto(x.Type!.Value, new Uri(x.Uri!), x.Quality!, x.Voices.Select(v => v.Name!).ToList()))
            .ToList();
        return new ChangeDto(filmInputModel.Id, filmInputModel.Description, filmInputModel.ShortDescription,
            uri, filmInputModel.Rating, cdns, filmInputModel.CountSeasons, filmInputModel.CountEpisodes,
            filmInputModel.NewPoster?.OpenReadStream());
    }

    public FilmViewModel Map(FilmShortDto dto) => new(dto.Id, dto.PosterUri, dto.Name, dto.Year);


    public LoadDto Map(AddFilmInputModel inputModel)
    {
        var uri = inputModel.PosterUrl == null ? null : new Uri(inputModel.PosterUrl);
        var cdnList = inputModel.Cdns
            .Select(x => new CdnDto(x.Type!.Value, new Uri(x.Uri!), x.Quality!, x.Voices.Select(s => s.Name!).ToList()))
            .ToList();
        var actors = inputModel.Actors.Select(x => new ValueTuple<string, string?>(x.Name!, x.Description)).ToList();
        var countries = inputModel.Countries.Select(x => x.Name).ToList();
        var genres = inputModel.Genres.Select(x => x.Name).ToList();
        var directors = inputModel.Directors.Select(x => x.Name).ToList();
        var screenwriters = inputModel.Screenwriters.Select(x => x.Name).ToList();
        return new LoadDto(inputModel.Name!, inputModel.Description!, inputModel.ShortDescription,
            inputModel.Rating!.Value, inputModel.Year!.Value, inputModel.Type!.Value, uri,
            inputModel.Poster?.OpenReadStream(), genres!,
            actors, countries!, directors!, screenwriters!, cdnList, inputModel.CountSeasons, inputModel.CountEpisodes);
    }

    public FilmInfoViewModel Map(FilmApiDto apiDto)
    {
        return new FilmInfoViewModel(apiDto.Title, apiDto.Description, apiDto.ShortDescription, apiDto.Rating, apiDto.Year, apiDto.Type,
            apiDto.PosterUrl, apiDto.Genres, apiDto.Actors.Select(x => new ActorViewModel(x.name, x.description)).ToList(),
            apiDto.Countries, apiDto.Directors, apiDto.Screenwriters, apiDto.CountSeasons,
            apiDto.CountEpisodes, apiDto.Cdn.Select(x => new CdnViewModel(x.Type, x.Uri, x.Quality, x.Voices)).ToList());
    }
}