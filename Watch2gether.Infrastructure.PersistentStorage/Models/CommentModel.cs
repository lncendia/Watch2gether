namespace Watch2gether.Infrastructure.PersistentStorage.Models;

public class CommentModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guid? UserId { get; set; }
    public UserModel? User { get; set; }

    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
}