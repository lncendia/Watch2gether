namespace Films.Infrastructure.Web.Comments.ViewModels;

public class CommentsViewModel
{
    public required IEnumerable<CommentViewModel> Comments { get; init; }
    public required int CountPages { get; init; }
}