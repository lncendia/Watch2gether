using Overoom.Application.Abstractions.StartPage.DTOs;

namespace Overoom.Application.Abstractions.StartPage.Interfaces;

public interface IStartPageService
{
    Task<StartInfoDto> GetAsync();
}