using Films.Application.Abstractions.Commands.Comments;
using Films.Application.Abstractions.DTOs.Comments;
using Films.Application.Abstractions.Queries.Comments;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Comments.InputModels;
using Films.Infrastructure.Web.Comments.ViewModels;
using Films.Infrastructure.Web.Common.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.Comments.Controllers;

[ApiController]
[Route("filmApi/[controller]")]
public class CommentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ListViewModel<CommentViewModel>> Get([FromQuery] GetCommentsInputModel model)
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

        return new ListViewModel<CommentViewModel>
        {
            List = data.comments.Select(c => Map(c, userId)),
            TotalPages = countPages
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
    public async Task<CommentViewModel> Add(AddCommentInputModel model)
    {
        var comment = await mediator.Send(new AddCommentCommand
        {
            FilmId = model.FilmId,
            UserId = User.GetId(),
            Text = model.Text!
        });

        return Map(comment, User.GetId());
    }


    private static CommentViewModel Map(CommentDto comment, Guid? userId) => new()
    {
        Id = comment.Id,
        Username = comment.Username,
        Text = comment.Text,
        AvatarUrl = comment.PhotoUrl?.ToString().Replace('\\', '/'),
        CreatedAt = comment.CreatedAt,
        IsUserComment = comment.UserId == userId
    };
}