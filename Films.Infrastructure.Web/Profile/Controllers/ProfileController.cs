using Films.Application.Abstractions.Commands.Profile;
using Films.Application.Abstractions.Commands.Ratings;
using Films.Application.Abstractions.DTOs.Profile;
using Films.Application.Abstractions.Queries.Profile;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Profile.InputModels;
using Films.Infrastructure.Web.Profile.ViewModels;
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
    public async Task<ProfileViewModel> Profile()
    {
        var profile = await mediator.Send(new UserProfileQuery
        {
            Id = User.GetId()
        });

        return Map(profile);
    }

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

    [HttpPut]
    public async Task AddToHistory(FilmIdInputModel model)
    {
        await mediator.Send(new AddToHistoryCommand
        {
            UserId = User.GetId(),
            FilmId = model.FilmId
        });
    }

    [HttpPost]
    public async Task ToggleWatchlist(FilmIdInputModel model)
    {
        await mediator.Send(new ToggleWatchListCommand
        {
            UserId = User.GetId(),
            FilmId = model.FilmId
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

    private UserRatingViewModel Map(UserRatingDto dto) => new()
    {
        FilmId = dto.Id,
        Title = dto.Title,
        Year = dto.Year,
        PosterUrl = $"{Request.Scheme}://{Request.Host}/{dto.PosterUrl.ToString().Replace('\\', '/')}",
        Score = dto.Score,
        RatingKp = dto.RatingKp,
        RatingImdb = dto.RatingImdb
    };

    private ProfileViewModel Map(UserProfileDto dto)
    {
        return new ProfileViewModel
        {
            Allows = new AllowsViewModel
            {
                Beep = dto.Allows.Beep,
                Scream = dto.Allows.Scream,
                Change = dto.Allows.Change
            },
            Watchlist = dto.Watchlist.Select(Map).ToArray(),
            History = dto.History.Select(Map).ToArray(),
            Genres = dto.Genres
        };
    }

    private UserFilmViewModel Map(UserFilmDto dto) => new()
    {
        FilmId = dto.Id,
        Title = dto.Title,
        Year = dto.Year,
        RatingKp = dto.RatingKp,
        RatingImdb = dto.RatingImdb,
        PosterUrl = $"{Request.Scheme}://{Request.Host}/{dto.PosterUrl.ToString().Replace('\\', '/')}"
    };
}