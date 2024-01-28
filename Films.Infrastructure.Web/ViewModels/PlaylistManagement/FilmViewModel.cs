namespace Films.Infrastructure.Web.Models.PlaylistManagement;

public class FilmViewModel
{
    public FilmViewModel(Guid id, string name, string description, Uri avatarUri)
    {
        Id = id;
        Name = name;
        Description = description;
        AvatarUri = avatarUri;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri AvatarUri { get; }
    public string Description { get; }
}