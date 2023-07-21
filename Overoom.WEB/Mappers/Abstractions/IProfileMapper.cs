using Overoom.Application.Abstractions.Profile.DTOs;
using Overoom.WEB.Models.Settings;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IProfileMapper
{
    ProfileViewModel Map(ProfileDto dto, IReadOnlyCollection<string> genres);
    RatingViewModel Map(RatingDto dto);
}