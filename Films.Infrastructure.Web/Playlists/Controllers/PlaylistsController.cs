using Films.Application.Abstractions.Queries.Playlists;
using Films.Application.Abstractions.Queries.Playlists.DTOs;
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
    public async Task<PlaylistsViewModel> Search(PlaylistsSearchInputModel model)
    {
        var data = await mediator.Send(new FindPlaylistsQuery
        {
            Genre = model.Genre,
            Query = model.Query,
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