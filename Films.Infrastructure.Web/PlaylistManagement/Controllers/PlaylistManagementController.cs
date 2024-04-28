using Films.Application.Abstractions.Commands.PlaylistManagement;
using Films.Infrastructure.Web.PlaylistManagement.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.PlaylistManagement.Controllers;

[ApiController]
[Authorize("admin")]
[Route("filmApi/[controller]")]
public class PlaylistManagementController(ISender mediator) : ControllerBase
{
    [HttpPut]
    public async Task Create(CreatePlaylistInputModel model)
    {
        await mediator.Send(new CreatePlaylistCommand
        {
            Name = model.Name!,
            Description = model.Description!,
            PosterUrl = string.IsNullOrEmpty(model.PosterUrl) ? null : new Uri(model.PosterUrl),
            PosterBase64 = model.PosterBase64
        });
    }

    [HttpPost]
    public async Task Edit(ChangePlaylistInputModel model)
    {
        await mediator.Send(new ChangePlaylistCommand
        {
            Id = model.Id,
            Description = model.Description,
            Name = model.Description,
            PosterBase64 = model.PosterBase64,
            PosterUrl = string.IsNullOrEmpty(model.PosterUrl) ? null : new Uri(model.PosterUrl),
            Films = model.Films?.Select(f => f.Id!.Value).ToArray()
        });
    }

    [HttpDelete("{playlistId:guid}")]
    public async Task Delete(Guid playlistId)
    {
        await mediator.Send(new DeletePlaylistCommand
        {
            Id = playlistId
        });
    }
}