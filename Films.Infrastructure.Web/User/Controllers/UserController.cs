using Films.Application.Abstractions.Commands.UserFilms;
using Films.Application.Abstractions.Commands.UserSettings;
using Films.Application.Abstractions.Queries.Users;
using Films.Application.Abstractions.Queries.Users.DTOs;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Films.InputModels;
using Films.Infrastructure.Web.User.InputModels;
using Films.Infrastructure.Web.User.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.User.Controllers;

[Authorize]
[ApiController]
[Route("filmApi/[controller]/[action]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<RatingsViewModel> Ratings([FromQuery] GetRatingsInputModel model)
    {
        var data = await mediator.Send(new UserRatingsQuery
        {
            Id = User.GetId(),
            Skip = (model.Page - 1) * model.CountPerPage,
            Take = model.CountPerPage
        });

        var countPages = data.count / model.CountPerPage;

        if (data.count % model.CountPerPage > 0) countPages++;

        return new RatingsViewModel
        {
            Ratings = data.ratings.Select(Map),
            CountPages = countPages
        };
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
    public async Task ChangeAllows(ChangeAllowsInputModel model)
    {
        await mediator.Send(new ChangeAllowsCommand
        {
            UserId = User.GetId(),
            Beep = model.Beep,
            Scream = model.Scream,
            Change = model.Change
        });
    }

    private static RatingViewModel Map(RatingDto dto) => new()
    {
        FilmId = dto.FilmId,
        Name = dto.Name,
        Year = dto.Year,
        PosterUrl = dto.PosterUrl,
        Score = dto.Score
    };
}