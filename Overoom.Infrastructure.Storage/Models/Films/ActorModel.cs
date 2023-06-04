namespace Overoom.Infrastructure.Storage.Models.Films;

public class ActorModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public FilmModel FilmModel { get; set; } = null!;
}