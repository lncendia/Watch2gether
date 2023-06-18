﻿namespace Overoom.Application.Abstractions.Films.Load.Interfaces;

public interface IFilmPosterService
{
    Task<Uri> SaveAsync(Uri url);
    Task DeleteAsync(Uri uri);
}