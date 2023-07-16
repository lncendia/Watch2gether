using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.WEB.Models.Settings;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IProfileMapper
{
    ProfileViewModel Map(ProfileDto dto);
    RatingViewModel Map(RatingDto dto);
}