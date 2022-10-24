using Overoom.Application.Abstractions.DTO.Comments;

namespace Overoom.Application.Abstractions.Interfaces.Comments;

public interface ICommentManager
{
    Task<List<CommentDto>> GetCommentsAsync(Guid filmId, int page);
    Task<CommentDto> AddCommentAsync(Guid filmId, string email, string text);
    Task UserDeleteCommentAsync(Guid commentId, string email);
    Task DeleteCommentAsync(Guid commentId);
}