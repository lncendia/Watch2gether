namespace Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

public class PlaylistDto
{
    public PlaylistDto(Guid id, string name, string description, Uri posterUri, IReadOnlyCollection<Guid> films)
    {
        Id = id;
        Name = name;
        Description = description;
        PosterUri = posterUri;
        Films = films;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public Uri PosterUri { get; }
    public IReadOnlyCollection<Guid> Films { get; }
}