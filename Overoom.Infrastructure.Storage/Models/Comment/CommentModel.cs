using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Film;
using Overoom.Infrastructure.Storage.Models.User;

namespace Overoom.Infrastructure.Storage.Models.Comment;

public class CommentModel : IAggregateModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? UserId { get; set; }
    public UserModel? User { get; set; }
    public Guid FilmId { get; set; }

    public FilmModel Film { get; set; } = null!;
}