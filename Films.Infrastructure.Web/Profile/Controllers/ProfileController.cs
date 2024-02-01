using Films.Application.Abstractions.Commands.Films;
using Films.Application.Abstractions.Queries.Ratings;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Films.InputModels;
using Films.Infrastructure.Web.Profile.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.Profile.Controllers;

[Authorize]
[ApiController]
[Route("filmApi/[controller]/[action]")]
public class ProfileController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Ratings(int page)
    {
        await mediator.Send(new UserRatingsQuery())
    }

    [HttpPut]
    public async Task AddRating(AddRatingInputModel model)
    {
        await mediator.Send(new AddRatingCommand
        {
            FilmId = model.FilmId,
            UserId = User.GetId(),
            Score = model.Score
        });
    }

    [HttpPost]
    public async Task ToggleWatchlist(Guid filmId)
    {
        await mediator.Send(new ToggleWatchListCommand
        {
            UserId = User.GetId(),
            FilmId = filmId
        });
    }

    [HttpPost]
    public async Task<ActionResult> ChangeAllows(ChangeAllowsInputModel model)
    {
        await mediator.Send(new 
        {
            UserId = User.GetId(),
            FilmId = filmId
        });
    }
}