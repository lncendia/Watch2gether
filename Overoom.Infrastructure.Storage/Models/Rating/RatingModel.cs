using Overoom.Infrastructure.Storage.Models.Abstractions;
using Overoom.Infrastructure.Storage.Models.Films;
using Overoom.Infrastructure.Storage.Models.Users;

namespace Overoom.Infrastructure.Storage.Models.Rating;

public class RatingModel : IAggregateModel
{
    public Guid Id { get; set; }
    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserModel UserModel { get; set; } = null!;
    public double Score { get; set; }
}