using Films.Application.Abstractions.DTOs.Playlists;
using Films.Application.Abstractions.Queries.Playlists;
using Films.Infrastructure.Web.Playlists.InputModels;
using Films.Infrastructure.Web.Playlists.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.Playlists.Controllers;

[ApiController]
[Route("filmApi/[controller]")]
public class PlaylistsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<PlaylistsViewModel> Search([FromQuery] PlaylistsSearchInputModel model)
    {
        var data = await mediator.Send(new FindPlaylistsQuery
        {
            Genre = model.Genre,
            Query = model.Query,
            FilmId = model.FilmId,
            Skip = (model.Page - 1) * model.CountPerPage,
            Take = model.CountPerPage
        });


        var countPages = data.count / model.CountPerPage;

        if (data.count % model.CountPerPage > 0) countPages++;

        return new PlaylistsViewModel
        {
            CountPages = countPages,
            Playlists = data.playlists.Select(Map)
        };
    }

    [HttpGet("{id:guid}")]
    public async Task<PlaylistViewModel> Get(Guid id)
    {
        var playlist = await mediator.Send(new PlaylistByIdQuery { Id = id });

        return Map(playlist);
    }

    private PlaylistViewModel Map(PlaylistDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Genres = dto.Genres,
        Description = dto.Description,
        PosterUrl = $"{Request.Scheme}://{Request.Host}/{dto.PosterUrl.ToString().Replace('\\', '/')}",
        Updated = dto.Updated
    };
}