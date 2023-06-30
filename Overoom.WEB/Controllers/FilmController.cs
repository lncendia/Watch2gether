using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Films.Catalog.Interfaces;
using Overoom.Application.Abstractions.Films.Playlist.DTOs;
using Overoom.Application.Abstractions.Films.Playlist.Interfaces;
using Overoom.WEB.Contracts.Films;
using Overoom.WEB.Models.Films;
using Overoom.WEB.RoomAuthentication;
using IFilmMapper = Overoom.WEB.Mappers.Abstractions.IFilmMapper;
using SortBy = Overoom.Application.Abstractions.Films.Playlist.DTOs.SortBy;

namespace Overoom.WEB.Controllers;

public class FilmController : Controller
{
    private readonly IFilmManager _filmManager;
    private readonly IPlaylistManager _playlistManager;
    private readonly ICommentManager _commentManager;
    private readonly IFilmMapper _filmMapper;

    public FilmController(IFilmManager manager, IPlaylistManager playlistManager, ICommentManager commentManager,
        IFilmMapper filmMapper)
    {
        _filmManager = manager;
        _playlistManager = playlistManager;
        _commentManager = commentManager;
        _filmMapper = filmMapper;
    }

    public IActionResult Index(string? person = null, string? genre = null,
        string? country = null)
    {
        var model = new FilmsSearchParameters
        {
            Genre = genre, Country = country, Person = person,
            SortBy = Application.Abstractions.Films.Catalog.DTOs.SortBy.Date
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchParameters model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var films = await _filmManager.FindAsync(_filmMapper.Map(model));
            if (!films.Any()) return NoContent();
            var filmsModels = films.Select(_filmMapper.Map).ToList();
            var viewModel = new FilmListViewModel(User.IsAdmin(), filmsModels);
            return Json(viewModel);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    public async Task<IActionResult> Film(Guid id)
    {
        var film = await _filmManager.GetAsync(id);
        var playlists =
            await _playlistManager.FindAsync(new PlaylistSearchQueryDto(null, SortBy.Date, 1, false));

        var playlistViewModels = playlists.Select(_filmMapper.Map).ToList();
        var filmViewModel = _filmMapper.Map(film);

        return View(new FilmPageViewModel(filmViewModel, playlistViewModels));
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
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Identity.Application")]
    public async Task<IActionResult> AddComment(Guid filmId, string text)
    {
        try
        {
            var comment = await _commentManager.AddAsync(filmId, User.GetId(), text);
            return Json(_filmMapper.Map(comment));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Authorize(Policy = "Identity.Application")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        try
        {
            var userId = User.GetId();
            await _commentManager.DeleteAsync(id, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AdminDeleteComment(Guid id)
    {
        try
        {
            await _commentManager.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // [HttpDelete]
    // [Authorize(Policy = "Admin")]
    // public async Task<IActionResult> Delete(Guid id)
    // {
    //     try
    //     {
    //         await _filmManager.DeleteAsync(id);
    //         return Ok();
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }
}