namespace Overoom.Infrastructure.Storage.Models.Film;

public class CountryModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string NameNormalized { get; set; } = null!;
    public FilmModel FilmModel { get; set; } = null!;
}