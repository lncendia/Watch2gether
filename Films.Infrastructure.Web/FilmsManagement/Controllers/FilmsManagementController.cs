using Films.Application.Abstractions.Commands.FilmsManagement;
using Films.Domain.Films.ValueObjects;
using Films.Infrastructure.Web.FilmsManagement.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.FilmsManagement.Controllers;

[ApiController]
[Authorize("admin")]
[Route("filmApi/[controller]")]
public class FilmsManagementController(ISender mediator) : ControllerBase
{
    [HttpPut]
    public async Task Add(AddFilmInputModel model)
    {
        await mediator.Send(new AddFilmCommand
        {
            Description = model.Description!,
            IsSerial = model.IsSerial!.Value,
            Title = model.Name!,
            Year = model.Year!.Value,
            RatingKp = model.RatingKp,
            RatingImdb = model.RatingImdb,
            PosterUrl = string.IsNullOrEmpty(model.PosterUrl) ? null : new Uri(model.PosterUrl),
            PosterBase64 = model.PosterBase64,
            CdnList = model.Cdns!.Select(c => new Cdn
            {
                Name = c.Name!,
                Quality = c.Quality!,
                Url = new Uri(c.Url!),
            }).ToArray(),
            Countries = model.Countries!.Select(c => c.Name!).ToArray(),
            Actors = model.Actors!.Select(a => new Actor(a.Name!, a.Description)).ToArray(),
            Directors = model.Directors!.Select(d => d.Name!).ToArray(),
            Genres = model.Genres!.Select(g => g.Name!).ToArray(),
            Screenwriters = model.Screenwriters!.Select(s => s.Name!).ToArray()
        });
    }

    [HttpPost]
    public async Task Edit(ChangeFilmInputModel model)
    {
        await mediator.Send(new ChangeFilmCommand
        {
            Id = model.Id,
            Description = model.Description,
            CountSeasons = model.CountSeasons,
            CountEpisodes = model.CountEpisodes,
            PosterUrl = string.IsNullOrEmpty(model.PosterUrl) ? null : new Uri(model.PosterUrl),
            PosterBase64 = model.PosterBase64,
            RatingKp = model.RatingKp,
            RatingImdb = model.RatingImdb,
            ShortDescription = model.ShortDescription,
            CdnList = model.Cdns?.Select(c => new Cdn
            {
                Name = c.Name!,
                Url = new Uri(c.Url!),
                Quality = c.Quality!,
            }).ToArray()
        });
    }

    [HttpDelete("{filmId:guid}")]
    public async Task Delete(Guid filmId)
    {
        await mediator.Send(new DeleteFilmCommand
        {
            Id = filmId
        });
    }
}