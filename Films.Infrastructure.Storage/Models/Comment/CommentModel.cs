using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Film;
using Films.Infrastructure.Storage.Models.User;

namespace Films.Infrastructure.Storage.Models.Comment;

public class CommentModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? UserId { get; set; }
    public UserModel? User { get; set; }
    public Guid FilmId { get; set; }

    public FilmModel Film { get; set; } = null!;
}