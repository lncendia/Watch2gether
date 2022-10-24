namespace Overoom.Infrastructure.PersistentStorage.Models.Films;

public class ScreenWriterModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public FilmModel FilmModel { get; set; } = null!;
}