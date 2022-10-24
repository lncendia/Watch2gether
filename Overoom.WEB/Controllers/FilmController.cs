using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.DTO.Films.FilmCatalog;
using Overoom.Application.Abstractions.DTO.Playlists;
using Overoom.Application.Abstractions.Exceptions.Films;
using Overoom.Application.Abstractions.Interfaces.Comments;
using Overoom.Application.Abstractions.Interfaces.Films;
using Overoom.Application.Abstractions.Interfaces.Playlists;
using Overoom.WEB.Models.Film;
using SortBy = Overoom.Application.Abstractions.DTO.Playlists.SortBy;

namespace Overoom.WEB.Controllers;

public class FilmController : Controller
{
    private readonly IFilmManager _filmManager;
    private readonly IPlaylistManager _playlistManager;
    private readonly ICommentManager _commentManager;

    public FilmController(IFilmManager manager, IPlaylistManager playlistManager, ICommentManager commentManager)
    {
        _filmManager = manager;
        _playlistManager = playlistManager;
        _commentManager = commentManager;
    }

    public IActionResult Index(string? query = null, string? person = null, string? genre = null,
        string? country = null)
    {
        var model = new FilmsSearchViewModel
        {
            Query = query, Genre = genre, Country = country, Person = person,
            SortBy = Overoom.Application.Abstractions.DTO.Films.FilmCatalog.SortBy.Date
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchViewModel model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var films = await _filmManager.GetFilmsAsync(new FilmSearchQueryDto(model.Query, model.MinYear,
                model.MaxYear, model.Genre, model.Country, model.Person, model.Type, model.SortBy, model.Page,
                model.InverseOrder));

            if (!films.Any()) return NoContent();

            var filmsModels = films.Select(x => new FilmLiteViewModel(x.Id, x.Name, x.PosterFileName, x.Rating,
                x.ShortDescription, x.Year, x.Type, x.CountSeasons, string.Join(", ", x.Genres)));

            var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            var isAdmin = data.Succeeded && data.Principal.IsInRole(ApplicationConstants.AdminRoleName);

            var viewModel = new FilmListViewModel(isAdmin, filmsModels);
            return Json(viewModel);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    public async Task<IActionResult> Film(Guid id)
    {
        try
        {
            var film = await _filmManager.GetFilmAsync(id);
            var playlists =
                await _playlistManager.GetPlaylists(new PlaylistSearchQueryDto(null, SortBy.Date, 1, false));

            var playlistViewModels =
                playlists.Select(x => new PlaylistLiteViewModel(x.Id, x.Name, x.PosterFileName)).ToList();
            var filmViewModel = new FilmViewModel(film.Id, film.Name, film.Year, film.Type, film.PosterFileName,
                film.Description,
                film.Rating, film.Directors, film.ScreenWriters, film.Genres, film.Countries, film.Actors,
                film.CountSeasons, film.CountEpisodes, film.Url);

            return View(new FilmPageViewModel(filmViewModel, playlistViewModels));
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                FilmNotFoundException => "Фильм не найден",
                _ => throw new ArgumentOutOfRangeException()
            };
            return RedirectToAction("Index", "Home", new {message = text});
        }
    }

    [HttpGet]
    public async Task<IActionResult> Comments(Guid id, int page)
    {
        try
        {
            var comments = await _commentManager.GetCommentsAsync(id, page);
            if (!comments.Any()) return NoContent();
            var commentModels = comments
                .Select(x => new CommentViewModel(x.Username, x.Text, x.CreatedAt, x.AvatarFileName))
                .ToList();
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
            var comment = await _commentManager.AddCommentAsync(filmId, HttpContext.User.Identity!.Name!, text);
            return Json(new CommentViewModel(comment.Username, comment.Text, comment.CreatedAt,
                comment.AvatarFileName));
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
            await _commentManager.UserDeleteCommentAsync(id, HttpContext.User.Identity!.Name!);
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
            await _commentManager.DeleteCommentAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _filmManager.DeleteFilmAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}