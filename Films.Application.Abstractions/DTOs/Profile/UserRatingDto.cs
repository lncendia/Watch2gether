namespace Films.Application.Abstractions.DTOs.Profile;

public class UserRatingDto : UserFilmDto
{
    public required double Score { get; init; }
}