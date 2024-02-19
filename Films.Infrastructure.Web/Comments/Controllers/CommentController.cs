using Films.Application.Abstractions.Commands.UserComments;
using Films.Application.Abstractions.Queries.Comments;
using Films.Application.Abstractions.Queries.Comments.DTOs;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Comments.InputModels;
using Films.Infrastructure.Web.Comments.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.Comments.Controllers;

[ApiController]
[Route("filmApi/[controller]")]
public class CommentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<CommentsViewModel> Get([FromQuery] GetCommentsInputModel model)
    {
        var data = await mediator.Send(new FilmCommentsQuery
        {
            FilmId = model.FilmId!.Value,
            Skip = (model.Page - 1) * model.CountPerPage,
            Take = model.CountPerPage
        });

        var countPages = data.count / model.CountPerPage;

        if (data.count % model.CountPerPage > 0) countPages++;

        Guid? userId = User.Identity is { IsAuthenticated: true } ? User.GetId() : null;

        return new CommentsViewModel
        {
            Comments = data.comments.Select(c => Map(c, userId)),
            CountPages = countPages
        };
    }

    [HttpDelete("{commentId:guid}")]
    [Authorize]
    public async Task Delete(Guid commentId)
    {
        await mediator.Send(new RemoveCommentCommand
        {
            CommentId = commentId,
            UserId = User.GetId()
        });
    }


    [HttpPut]
    [Authorize]
    public async Task Add(AddCommentInputModel model)
    {
        await mediator.Send(new AddCommentCommand
        {
            FilmId = model.FilmId,
            UserId = User.GetId(),
            Text = model.Text!
        });
    }


    private static CommentViewModel Map(CommentDto comment, Guid? userId) => new()
    {
        Id = comment.Id,
        Username = comment.Username,
        Text = comment.Text,
        AvatarUrl = comment.PhotoUrl,
        CreatedAt = comment.CreatedAt,
        IsUserComment = comment.UserId == userId
    };
}