namespace Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

public class ChangeDto
{
    public ChangeDto(Guid id, string? name, string? description, Uri? posterUri, Stream? posterStream,
        IReadOnlyCollection<Guid>? films)
    {
        Description = description;
        PosterUri = posterUri;
        PosterStream = posterStream;
        Films = films;
        Name = name;
        Id = id;
    }

    public Guid Id { get; }
    public string? Name { get; }
    public string? Description { get; }
    public Uri? PosterUri { get; }
    public Stream? PosterStream { get; }
    public IReadOnlyCollection<Guid>? Films { get; }
}