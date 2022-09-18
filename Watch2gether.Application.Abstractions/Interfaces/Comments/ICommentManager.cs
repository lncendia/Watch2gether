using Watch2gether.Application.Abstractions.DTO.Comments;

namespace Watch2gether.Application.Abstractions.Interfaces.Comments;

public interface ICommentManager
{
    Task<List<CommentDto>> GetCommentsAsync(Guid filmId, int page);
    Task<CommentDto> AddCommentAsync(Guid filmId, string email, string text);
}