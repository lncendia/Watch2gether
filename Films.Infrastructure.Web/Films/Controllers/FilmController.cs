using Films.Application.Abstractions.Queries.Films;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Films.Infrastructure.Web.Contracts.Film;
using Films.Infrastructure.Web.Models.Film;
using Films.Infrastructure.Web.Authentication;
using MediatR;

namespace Films.Infrastructure.Web.Controllers;

[ApiController]
[Route("filmApi/[controller]")]
public class FilmController(IMediator mediator) : ControllerBase
{
    
    public async Task<IActionResult> Films()
    {
        var popular = await mediator.Send(new PopularFilmsQuery
        {
            Take = 10
        });
        var vm = new FilmsViewModel(popular.Select(_filmsMapper.MapShort).ToList());
        return View(vm);
    }

    public async Task<IActionResult> FilmSearch(SearchParameters model)
    {
        
        if (search.PlaylistId.HasValue)
            ViewData["playlist"] = await _filmsManager.GetPlaylistNameAsync(search.PlaylistId.Value);
        return View(search);
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchParameters model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var films = await _filmsManager.FindAsync(_filmsMapper.Map(model));
            if (!films.Any()) return NoContent();
            var filmsModels = films.Select(_filmsMapper.Map).ToList();
            return Json(filmsModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    public async Task<IActionResult> Get(Guid id)
    {
        Guid? userId = User.Identity is { IsAuthenticated: true } ? User.GetId() : null;

        var film = await mediator.Send(new FilmByIdQuery
        {
            Id = id,
            UserId = userId
        });

        return View(new FilmPageViewModel(filmViewModel, null!));
    }
    

    [HttpPost]
    [Authorize(Policy = "User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRating(AddRatingInputModel model)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var rating = await _filmManager.AddRatingAsync(model.FilmId, User.GetId(), model.Score);
            return Json(new RatingViewModel(rating.Rating, rating.Count));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> Watchlist(Guid filmId)
    {
        try
        {
            await _filmManager.ToggleWatchlistAsync(filmId, User.GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}