namespace Overoom.Infrastructure.Storage.Models.Film;

public class ActorModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public FilmModel FilmModel { get; set; } = null!;
}