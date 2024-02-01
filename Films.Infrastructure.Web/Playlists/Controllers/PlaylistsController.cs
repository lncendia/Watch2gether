using Films.Application.Abstractions.Queries.Playlists;
using Films.Infrastructure.Web.Playlists.InputModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.Playlists.Controllers;

[ApiController]
[Route("filmApi/[controller]")]
public class PlaylistsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search(PlaylistsSearchInputModel model)
    {
        var playlists = await mediator.Send(new FindPlaylistsQuery
        {
            Genre = model.Genre,
            Query = model.Query,
            Skip = (model.Page - 1) * model.CountPerPage,
            Take = model.CountPerPage
        });
    }
}