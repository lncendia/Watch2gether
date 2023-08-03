namespace Overoom.Infrastructure.Storage.Models.Genre;

public class GenreModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string NameNormalized { get; set; } = null!;
}