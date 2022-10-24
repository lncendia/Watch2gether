using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Interfaces.StartPage;
using Overoom.WEB.Models;
using Overoom.WEB.Models.Home;

namespace Overoom.WEB.Controllers;

public class HomeController : Controller
{
    private IStartPageService _startPageService;

    public HomeController(IStartPageService startPageService)
    {
        _startPageService = startPageService;
    }

    public async Task<IActionResult> Index(string? message)
    {
        ViewData["Alert"] = message;
        var info = await _startPageService.GetStartInfoAsync();
        var films = info.Films.Select(x => new FilmStartPageViewModel(x.Name, x.PosterUrl, x.Id, x.Genres));
        var comments = info.Comments.Select(x =>
            new CommentStartPageViewModel(x.Name, x.Text, x.DateTime, x.FilmId, x.Avatar));
        var rooms = info.Rooms.Select(x => new RoomStartPageViewModel(x.Id, x.Type, x.CountUsers, x.NowPlaying));
        var model = new StartPageViewModel(comments, films, rooms);
        return PartialView(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}