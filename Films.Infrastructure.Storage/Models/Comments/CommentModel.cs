using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Films;
using Films.Infrastructure.Storage.Models.Users;

namespace Films.Infrastructure.Storage.Models.Comments;

public class CommentModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [MaxLength(1000)] public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? UserId { get; set; }
    public UserModel? User { get; set; }
    public Guid FilmId { get; set; }

    public FilmModel Film { get; set; } = null!;
}