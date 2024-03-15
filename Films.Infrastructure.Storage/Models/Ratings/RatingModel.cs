using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Films;
using Films.Infrastructure.Storage.Models.Users;

namespace Films.Infrastructure.Storage.Models.Ratings;

public class RatingModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
    public Guid? UserId { get; set; }
    public UserModel? User { get; set; }
    public double Score { get; set; }
    public DateTime Date { get; set; }
}