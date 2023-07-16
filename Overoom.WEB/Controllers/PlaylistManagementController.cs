using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.WEB.Contracts.FilmManagement;
using IFilmManagementMapper = Overoom.WEB.Mappers.Abstractions.IFilmManagementMapper;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "Admin")]
public class PlaylistManagementController : Controller
{
    
}