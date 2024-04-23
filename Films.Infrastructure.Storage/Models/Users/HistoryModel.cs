using Films.Infrastructure.Storage.Models.Films;

namespace Films.Infrastructure.Storage.Models.Users;

public class HistoryModel
{
    public long Id { get; set; }
    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
    public DateTime Date { get; set; }
    public UserModel User { get; set; } = null!;
}