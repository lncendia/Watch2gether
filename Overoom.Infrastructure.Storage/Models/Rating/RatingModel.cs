using System.ComponentModel.DataAnnotations.Schema;
using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Film;
using Overoom.Infrastructure.Storage.Models.User;

namespace Overoom.Infrastructure.Storage.Models.Rating;

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