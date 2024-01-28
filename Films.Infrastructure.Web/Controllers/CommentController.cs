using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Films.Infrastructure.Web.Contracts.Film;
using Films.Infrastructure.Web.Authentication;
using MediatR;

namespace Films.Infrastructure.Web.Controllers;

[ApiController]
[Route("filmApi/[controller]")]
public class CommentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(Guid filmId, int page)
    {
        var comments = await _commentManager.GetAsync(filmId, page);
        if (comments.Count == 0) return NoContent();

        var auth = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        Guid? userId = auth.None ? null : auth.Principal!.GetId();
        var commentModels = comments.Select(c => _filmMapper.Map(c, userId)).ToList();
        return Json(commentModels);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(Guid commentId)
    {
        try
        {
            await _commentManager.DeleteAsync(commentId, User.GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Add(AddCommentInputModel model)
    {
        if (!ModelState.IsValid) return BadRequest();
        var id = User.GetId();
        try
        {
            var comment = await _commentManager.AddAsync(model.FilmId, User.GetId(), model.Text!);
            return Json(_filmMapper.Map(comment, id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}