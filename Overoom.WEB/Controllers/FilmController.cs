using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.WEB.Contracts.Film;
using Overoom.WEB.Models.Film;
using Overoom.WEB.RoomAuthentication;
using IFilmMapper = Overoom.WEB.Mappers.Abstractions.IFilmMapper;

namespace Overoom.WEB.Controllers;

public class FilmController : Controller
{
    private readonly IFilmManager _filmManager;
    private readonly IFilmMapper _filmMapper;
    private readonly ICommentManager _commentManager;

    public FilmController(IFilmManager manager, IFilmMapper filmMapper, ICommentManager commentManager)
    {
        _filmManager = manager;
        _filmMapper = filmMapper;
        _commentManager = commentManager;
    }

    public async Task<IActionResult> Index(Guid id)
    {
        Guid? userId = null;
        try
        {
            userId = User.GetId();
        }
        catch
        {
            // ignored
        }

        var film = await _filmManager.GetAsync(id, userId);
        // var playlists =
        //     await _playlistManager.FindAsync(new PlaylistSearchQuery(null, SortBy.Date, 1, false));
        //
        // var playlistViewModels = playlists.Select(_filmMapper.Map).ToList();
        var filmViewModel = _filmMapper.Map(film);

        return View(new FilmPageViewModel(filmViewModel, null!));
    }


    [HttpGet]
    public async Task<IActionResult> GetFilmUri(GetUriParameters model)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var uri = await _filmManager.GetFilmUriAsync(model.Id, model.CdnType);
            return Json(new FilmUrlViewModel(uri));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Comments(Guid id, int page)
    {
        try
        {
            var comments = await _commentManager.GetAsync(id, page);
            if (!comments.Any()) return NoContent();
            var commentModels = comments.Select(_filmMapper.Map).ToList();
            return Json(commentModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Authorize(Policy = "User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRating(AddRatingParameters model)
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


    [HttpPost]
    [Authorize(Policy = "User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        try
        {
            await _commentManager.DeleteAsync(id, User.GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> AddComment(AddCommentParameters model)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var comment = await _commentManager.AddAsync(model.FilmId, User.GetId(), model.Text!);
            return Json(_filmMapper.Map(comment));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}