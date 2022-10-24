using Overoom.Infrastructure.PersistentStorage.Models.Films;
using Overoom.Infrastructure.PersistentStorage.Models.Users;

namespace Overoom.Infrastructure.PersistentStorage.Models.Comments;

public class CommentModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid FilmId { get; set; }
}