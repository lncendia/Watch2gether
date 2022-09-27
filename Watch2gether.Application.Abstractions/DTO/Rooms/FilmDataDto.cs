namespace Watch2gether.Application.Abstractions.DTO.Rooms;

public class FilmDataDto
{
    public FilmDataDto(Guid id, string name, string url)
    {
        Name = name;
        Url = url;
        Id = id;
    }

    public Guid Id { get; }

    public string Name { get; }
    public string Url { get; }
}