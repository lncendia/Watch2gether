namespace Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

public class LoadDto
{
    public LoadDto(string name, string description, Uri? posterUri, Stream? posterStream,
        IReadOnlyCollection<Guid> films)
    {
        Description = description;
        PosterUri = posterUri;
        PosterStream = posterStream;
        Films = films;
        Name = name;
    }

    public string Name { get; }
    public string Description { get; }
    public Uri? PosterUri { get; }
    public Stream? PosterStream { get; }
    public IReadOnlyCollection<Guid> Films { get; }
}