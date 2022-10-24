using Overoom.Application.Abstractions.DTO.StartPage;

namespace Overoom.Application.Abstractions.Interfaces.StartPage;

public interface IStartPageService
{
    Task<StartInfoDto> GetStartInfoAsync();
}