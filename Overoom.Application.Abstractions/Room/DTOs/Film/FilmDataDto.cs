﻿using Overoom.Domain.Film.Enums;

namespace Overoom.Application.Abstractions.Room.DTOs.Film;

public class FilmDataDto
{
    public FilmDataDto(Guid id, string name, Uri url, FilmType type)
    {
        Name = name;
        Url = url;
        Type = type;
        Id = id;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri Url { get; }
    public FilmType Type { get; }
}